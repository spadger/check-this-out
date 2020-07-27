using System;
using System.Threading.Tasks;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.PaymentStore;
using JonBates.CheckThisOut.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JonBates.CheckThisOut.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentProcess _paymentProcess;
        private readonly IPaymentStore _paymentStore;

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

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var request = requestDTO.ToRequest(Guid.Empty);

            var result = await _paymentProcess.ProcessAsync(request);

            _logger.LogInformation("<< Submitting a payment - submitted");

            return result switch
            {
                {IsRight: true} => Ok(new CaptureFundsSuccessResponseDTO(result.RightValue)),
                {IsLeft: true} when result.LeftValue.ErrorType == PaymentProcessErrorType.TransactionAlreadyExists => Conflict(),
                _ => new ObjectResult(new CaptureFundsErrorResponseDTO(result.LeftValue))
                                        {
                                            StatusCode = result.LeftValue.ErrorType.ToStatusCode()
                                        }
            };
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
