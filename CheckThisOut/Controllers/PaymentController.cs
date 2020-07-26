using System;
using System.Threading.Tasks;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.PaymentStore;
using JonBates.CheckThisOut.DTOs;
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

        [HttpPost]
        public async Task PostTransactionAsync(CaptureFundsRequestDTO requestDTO)
        {
            _logger.LogInformation(">> Submitting a payment");

            var request = requestDTO.ToRequest(Guid.Empty);

            await _paymentProcess.ProcessAsync(request);

            _logger.LogInformation("<< Submitting a payment - submitted");
        }

        [HttpGet, Route("{paymentRequestId}")]
        public async Task<IActionResult> RetrieveTransactionAsync(string paymentRequestId)
        {
            _logger.LogInformation($">> Retrieving a stored payment {paymentRequestId}");

            var result = await _paymentStore.RetrievePaymentDetailsAsync(paymentRequestId);

            if (result == null)
            {
                _logger.LogInformation(">>Retrieving a stored payment - not found");
                return NotFound();

            }

            var responseDto = new SubmittedPaymentDTO(result);

            _logger.LogInformation(">>Retrieving a stored payment - retrieved");

            return Ok(responseDto);
        }
    }
}
