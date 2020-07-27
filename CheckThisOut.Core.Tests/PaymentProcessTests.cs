using System.Threading.Tasks;
using FluentAssertions;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.BankClient;
using JonBates.CheckThisOut.Core.PaymentStore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CheckThisOut.Core.Tests
{
    public class PaymentProcessTests
    {
        private readonly Mock<IPaymentStore> _paymentStore;
        private readonly Mock<IBankClient> _bankClient;

        private readonly PaymentProcess sut;
        public PaymentProcessTests()
        {
            var log = Mock.Of<ILogger<PaymentProcess>>();
            _paymentStore = new Mock<IPaymentStore>();
            _bankClient = new Mock<IBankClient>();

            sut = new PaymentProcess(log, _paymentStore.Object, _bankClient.Object);
        }

        [Fact]
        public async Task valid_requests_are_processed_and_all_responses_stored()
        {
            var request = CaptureFundsMessageBuilder.BuildRequest();
            var expectedResult = CaptureFundsMessageBuilder.BuildSuccessfulResponse();

            _paymentStore.Setup(x => x.StoreCaptureFundsRequestAsync(request))
                .ReturnsAsync(StorePaymentRequestResult.StoredSuccessfully);

            _bankClient.Setup(x => x.CaptureFundsAsync(request)).ReturnsAsync(expectedResult);

            var result = await sut.ProcessAsync(request);

            result.IsRight.Should().BeTrue();

            result.Should().BeEquivalentTo(expectedResult);

            _paymentStore.Verify(x => x.StoreCaptureFundsResponseAsync(request.RequestId, expectedResult));
        }

        [Fact]
        public async Task invalid_requests_are_processed_and_all_responses_stored()
        {
            var request = CaptureFundsMessageBuilder.BuildRequest();
            var expectedResult = CaptureFundsMessageBuilder.BuildUnsuccessfulResponse();

            _paymentStore.Setup(x => x.StoreCaptureFundsRequestAsync(request))
                .ReturnsAsync(StorePaymentRequestResult.StoredSuccessfully);

            _bankClient.Setup(x => x.CaptureFundsAsync(request)).ReturnsAsync(expectedResult);

            var result = await sut.ProcessAsync(request);

            result.IsLeft.Should().BeTrue();

            result.Should().BeEquivalentTo(expectedResult);

            _paymentStore.Verify(x => x.StoreCaptureFundsResponseAsync(request.RequestId, expectedResult));
        }

        [Fact]
        public async Task duplicate_requests_are_not_reprocessed()
        {
            var request = CaptureFundsMessageBuilder.BuildRequest();


            _paymentStore.Setup(x => x.StoreCaptureFundsRequestAsync(request))
                .ReturnsAsync(StorePaymentRequestResult.AlreadyExists);

            
            var result = await sut.ProcessAsync(request);

            result.IsLeft.Should().BeTrue();

            result.LeftValue.ErrorType.Should().Be(PaymentProcessErrorType.TransactionAlreadyExists);


            _bankClient.Verify(x => x.CaptureFundsAsync(It.IsAny<CaptureFundsRequest>()), Times.Never);
            _paymentStore.Verify(x=> x.StoreCaptureFundsResponseAsync(
                It.IsAny<string>(), 
                It.IsAny<Either<PaymentProcessErrorResult, CaptureFundsBankResponse>>()), Times.Never);

        }
    }
}