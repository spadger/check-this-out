using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.PaymentStore;
using JonBates.CheckThisOut.Core.Tests;
using JonBates.CheckThisOut.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JonBates.CheckThisOut.Tests.Controllers.PaymentController
{
    public class RetrievingAPaymentTests
    {
        private readonly ILogger<CheckThisOut.Controllers.PaymentController> _log = Mock.Of<ILogger<CheckThisOut.Controllers.PaymentController>>();
        private readonly Mock<IPaymentStore> _paymentStore;

        private const string RequestId = "some-request-id";

        public RetrievingAPaymentTests()
        {
            _paymentStore = new Mock<IPaymentStore>();
        }

        [Fact]
        public async Task Querying_a_nonexistent_payment_yields_a_404()
        {
            _paymentStore.Setup(x => x.RetrievePaymentDetailsAsync(RequestId)).ReturnsAsync((SubmittedPaymentDetails)null);

            var sut = new CheckThisOut.Controllers.PaymentController(_log, null, _paymentStore.Object);

            var result = await sut.RetrieveTransactionAsync(RequestId);

            result.As<StatusCodeResult>().StatusCode.Should().Be(404);

            _paymentStore.Verify(x => x.RetrievePaymentDetailsAsync(RequestId));
        }

        [Fact]
        public async Task Querying_a_stored_payment_yields_a_200_with_masked_payload()
        {
            var request = CaptureFundsMessageBuilder.BuildRequest(RequestId);
            var storedStatus = new SubmittedPaymentDetails(request, SubmittedPaymentProcessingStatus.Successful);

            _paymentStore.Setup(x => x.RetrievePaymentDetailsAsync(RequestId)).ReturnsAsync(storedStatus);

            var sut = new CheckThisOut.Controllers.PaymentController(_log, null, _paymentStore.Object);

            var result = await sut.RetrieveTransactionAsync(RequestId) as OkObjectResult;

            var response = result.Value.As<SubmittedPaymentDetailsResponseDTO>();
            response.MaskedPAN.Should().Be("************" + request.PAN[^4..]);

            _paymentStore.Verify(x => x.RetrievePaymentDetailsAsync(RequestId));
        }
    }
}