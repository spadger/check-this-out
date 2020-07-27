using System;
using System.Threading.Tasks;
using FluentAssertions;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.Tests;
using JonBates.CheckThisOut.DTOs;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JonBates.CheckThisOut.Tests.Controllers.PaymentController
{
    public class SubmittingAPaymentTests
    {
        private readonly ILogger<CheckThisOut.Controllers.PaymentController> _log = Mock.Of<ILogger<CheckThisOut.Controllers.PaymentController>>();
        private readonly Mock<IPaymentProcess> _paymentProcess;

        public SubmittingAPaymentTests()
        {
            _paymentProcess = new Mock<IPaymentProcess>();
        }

        [Fact]
        public async Task An_invalid_model_will_not_be_submitted_to_the_payment_process_and_will_yield_a_bad_request()
        {
            var sut = new CheckThisOut.Controllers.PaymentController(_log, _paymentProcess.Object, null);
            sut.ModelState.AddModelError("some_key", "some_message");

            var request = new CaptureFundsRequestDTO();

            var result = await sut.PostTransactionAsync(request);

            result.As<IStatusCodeActionResult>().StatusCode.Should().Be(400);

            _paymentProcess.Verify(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()), Times.Never);
        }

        [Fact]
        public async Task A_valid_request_will_be_submitted_to_the_payment_process_and_will_yield_a_successful_response()
        {
            var paymentProcessResult = CaptureFundsMessageBuilder.BuildSuccessfulResponse();
            _paymentProcess.Setup(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()))
                .ReturnsAsync(paymentProcessResult);

            var sut = new CheckThisOut.Controllers.PaymentController(_log, _paymentProcess.Object, null);
            var request = GetValidRequest();

            var result = await sut.PostTransactionAsync(request);

            result.As<IStatusCodeActionResult>().StatusCode.Should().Be(200);

            _paymentProcess.Verify(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()), Times.Once);
        }

        [Fact]
        public async Task A_repeat_request_submitted_to_the_payment_process_and_will_yield_a_conflict_response()
        {
            var paymentProcessResult = CaptureFundsMessageBuilder.BuildUnsuccessfulResponse(PaymentProcessErrorType.TransactionAlreadyExists);
            _paymentProcess.Setup(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()))
                .ReturnsAsync(paymentProcessResult);

            var sut = new CheckThisOut.Controllers.PaymentController(_log, _paymentProcess.Object, null);
            var request = GetValidRequest();

            var result = await sut.PostTransactionAsync(request);

            result.As<IStatusCodeActionResult>().StatusCode.Should().Be(409);

            _paymentProcess.Verify(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()), Times.Once);
        }

        [Fact]
        public async Task A_request_failing_upstream_validation_will_yield_a_bad_request_response()
        {
            var paymentProcessResult = CaptureFundsMessageBuilder.BuildUnsuccessfulResponse(PaymentProcessErrorType.AcquiringBankValidationError);
            _paymentProcess.Setup(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()))
                .ReturnsAsync(paymentProcessResult);

            var sut = new CheckThisOut.Controllers.PaymentController(_log, _paymentProcess.Object, null);
            var request = GetValidRequest();

            var result = await sut.PostTransactionAsync(request);

            result.As<IStatusCodeActionResult>().StatusCode.Should().Be(400);

            _paymentProcess.Verify(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()), Times.Once);
        }

        [Fact]
        public async Task A_requdst_that_runs_into_upstream_technical_issues_will_yield_an_error_response()
        {
            var paymentProcessResult = CaptureFundsMessageBuilder.BuildUnsuccessfulResponse(PaymentProcessErrorType.Exception);
            _paymentProcess.Setup(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()))
                .ReturnsAsync(paymentProcessResult);

            var sut = new CheckThisOut.Controllers.PaymentController(_log, _paymentProcess.Object, null);
            var request = GetValidRequest();

            var result = await sut.PostTransactionAsync(request);

            result.As<IStatusCodeActionResult>().StatusCode.Should().Be(500);

            _paymentProcess.Verify(x => x.ProcessAsync(It.IsAny<CaptureFundsRequest>()), Times.Once);
        }

        private CaptureFundsRequestDTO GetValidRequest()
        {
            return new CaptureFundsRequestDTO
            {
                RequestId = "req123",
                CustomerName = "John Smith",
                Address = "Some Address",
                PostCode = "SS1 1SS",
                PAN = "1234123412341234",
                Amount = 123,
                Currency = "GBP",
                ValidFrom = DateTime.Now.AddYears(-1),
                ValidTo = DateTime.Now.AddYears(2),
                CVV = "123",
                ThreeDSToken = "some-token",
                PostingDate = DateTime.Now,
            };
        }
    }
}
