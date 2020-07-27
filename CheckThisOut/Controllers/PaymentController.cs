using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.PaymentStore;
using JonBates.CheckThisOut.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace JonBates.CheckThisOut.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentProcess _paymentProcess;
        private readonly IPaymentStore _paymentStore;

        private static readonly Counter _requestCounter;
        private static readonly Counter _successCounter;
        private static readonly Summary _responseTimeSummary;

        static PaymentController()
        {
            _requestCounter = Metrics.CreateCounter(
                "payment_request_total_count",
                "The total number of new payment requests seen");

            _successCounter = Metrics.CreateCounter(
                "payment_request_success_count",
                "The total number of sucessfully processed payment requests");

            _responseTimeSummary = Metrics.CreateSummary("payment_request_duration_seconds",
                "The duration in seconds between the response to a new payment request", new SummaryConfiguration
                {
                    Objectives = new []
                    {
                        new QuantileEpsilonPair(0.5, 0.05),
                        new QuantileEpsilonPair(0.75, 0.05),
                        new QuantileEpsilonPair(0.9, 0.05),
                        new QuantileEpsilonPair(0.95, 0.01),
                        new QuantileEpsilonPair(0.99, 0.005),
                    }
                });
        }

        public PaymentController(ILogger<PaymentController> logger, IPaymentProcess paymentProcess, IPaymentStore paymentStore)
        {
            _logger = logger;
            _paymentProcess = paymentProcess;
            _paymentStore = paymentStore;
        }


        /// <summary>
        /// Submit a transaction for processing
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(CaptureFundsSuccessResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type=typeof(CaptureFundsErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostTransactionAsync(CaptureFundsRequestDTO requestDTO)
        {
            _logger.LogInformation(">> Submitting a payment");

            _requestCounter.Inc();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var request = requestDTO.ToRequest(Guid.Empty);

            var sw = Stopwatch.StartNew();
            
            var result = await _paymentProcess.ProcessAsync(request);

            _responseTimeSummary.Observe((double)sw.ElapsedMilliseconds/1000);

            _logger.LogInformation("<< Submitting a payment - submitted");

            IStatusCodeActionResult response = result switch
            {
                {IsRight: true} => Ok(new CaptureFundsSuccessResponseDTO(result.RightValue)),
                {IsLeft: true} when result.LeftValue.ErrorType == PaymentProcessErrorType.TransactionAlreadyExists => Conflict(),
                _ => new ObjectResult(new CaptureFundsErrorResponseDTO(result.LeftValue))
                                        {
                                            StatusCode = result.LeftValue.ErrorType.ToStatusCode()
                                        }
            };

            if (response.StatusCode == (int) HttpStatusCode.OK)
            {
                _successCounter.Inc();
            }

            return response;
        }


        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubmittedPaymentDetailsResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet, Route("{paymentRequestId}")]
        public async Task<IActionResult> RetrieveTransactionAsync(string paymentRequestId)
        {
            _logger.LogInformation($">> Retrieving a stored payment {paymentRequestId}");

            var result = await _paymentStore.RetrievePaymentDetailsAsync(paymentRequestId);

            if (result == null)
            {
                _logger.LogInformation(">> Retrieving a stored payment - not found");
                return NotFound();
            }

            var responseDto = new SubmittedPaymentDetailsResponseDTO(result);

            _logger.LogInformation(">> Retrieving a stored payment - retrieved");

            return Ok(responseDto);
        }
    }
}
