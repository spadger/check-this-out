using System.Threading.Tasks;
using FluentAssertions;
using JonBates.CheckThisOut.Core;
using JonBates.CheckThisOut.Core.BankClient;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CheckThisOut.Core.Tests.BankClient
{
    public class FakeBankClientTests
    {
        [Fact]
        public async Task The_client_can_return_successful_responses()
        {
            var sut = new FakeBankClient(Mock.Of<ILogger<FakeBankClient>>());

            var request = CaptureFundsMessageBuilder.BuildRequest("123");
            var result = await sut.CaptureFundsAsync(request);

            result.IsRight.Should().BeTrue();
            result.RightValue.ResponseId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task The_client_can_return_validation_error_responses()
        {
            var sut = new FakeBankClient(Mock.Of<ILogger<FakeBankClient>>());

            var request = CaptureFundsMessageBuilder.BuildRequest(cvv: FakeBankClient.CVVTypes.ValidationFailure);
            var result = await sut.CaptureFundsAsync(request);

            result.IsLeft.Should().BeTrue();
            var resultPayload = result.LeftValue;

            resultPayload.ErrorType.Should().Be(PaymentProcessErrorType.AcquiringBankValidationError);
            resultPayload.ExceptionId.Should().BeNull();
            resultPayload.FieldErrors.Should().NotBeEmpty();
        }


        [Fact]
        public async Task The_client_can_return_exception_error_responses()
        {
            var sut = new FakeBankClient(Mock.Of<ILogger<FakeBankClient>>());

            var request = CaptureFundsMessageBuilder.BuildRequest(cvv: FakeBankClient.CVVTypes.Exception);
            var result = await sut.CaptureFundsAsync(request);

            result.IsLeft.Should().BeTrue();
            var resultPayload = result.LeftValue;

            resultPayload.ErrorType.Should().Be(PaymentProcessErrorType.Exception);
            resultPayload.ExceptionId.Should().NotBeEmpty();
            resultPayload.FieldErrors.Should().BeEmpty();
        }
    }
}
