using System;
using System.Threading.Tasks;
using FluentAssertions;
using JonBates.CheckThisOut.Core.PaymentStore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JonBates.CheckThisOut.Core.Tests.PaymentStore
{
    public class InMemoryPaymentStoreTests
    {
        private static readonly ILogger<InMemoryPaymentStore> logger = Mock.Of<ILogger<InMemoryPaymentStore>>();
        
        [Fact]
        public async Task Querying_for_non_existent_payment_details_yields_null()
        {
            var sut = new InMemoryPaymentStore(logger);

            var result = await sut.RetrievePaymentDetailsAsync("nothing");

            result.Should().BeNull();
        }

        [Fact]
        public async Task Registering_a_new_capture_funds_request_is_successful()
        {
            var sut = new InMemoryPaymentStore(logger);
            var request = CaptureFundsMessageBuilder.BuildRequest();

            var result = await sut.StoreCaptureFundsRequestAsync(request);

            result.Should().Be(StorePaymentRequestResult.StoredSuccessfully);
            var saved = await sut.RetrievePaymentDetailsAsync(request.RequestId);
            saved.Request.Should().Be(request);
            saved.ProcessingStatus.Should().Be(SubmittedPaymentProcessingStatus.Pending);
        }

        [Fact]
        public async Task Registering_a_previously_seen_capture_funds_request_is_not_accepted()
        {
            var requestId = "some-id";
            var sut = new InMemoryPaymentStore(logger);

            var first = CaptureFundsMessageBuilder.BuildRequest(requestId, "first");
            var second = CaptureFundsMessageBuilder.BuildRequest(requestId, "second");

            var firstResult = await sut.StoreCaptureFundsRequestAsync(first);

            var secondResult = await sut.StoreCaptureFundsRequestAsync(second);

            firstResult.Should().Be(StorePaymentRequestResult.StoredSuccessfully);
            secondResult.Should().Be(StorePaymentRequestResult.AlreadyExists);

            var saved = await sut.RetrievePaymentDetailsAsync(requestId);
            saved.Request.Should().Be(first);

            saved.ProcessingStatus.Should().Be(SubmittedPaymentProcessingStatus.Pending);
        }

        [Fact]
        public async Task It_is_not_possible_to_store_a_response_without_a_request()
        {
            var requestId = "some-id";
            var sut = new InMemoryPaymentStore(logger);

            var response = CaptureFundsMessageBuilder.BuildSuccessfulResponse();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await sut.StoreCaptureFundsResponseAsync("some-request_id", response));
        }

        [Fact]
        public async Task The_store_accepts_valid_responses_and_updates_the_processing_status_accordingly()
        {
            var requestId = "some-id";
            var sut = new InMemoryPaymentStore(logger);
            var request = CaptureFundsMessageBuilder.BuildRequest(requestId);
            var response = CaptureFundsMessageBuilder.BuildSuccessfulResponse();

            await sut.StoreCaptureFundsRequestAsync(request);
            await sut.StoreCaptureFundsResponseAsync(requestId, response);

            var report = await sut.RetrievePaymentDetailsAsync(requestId);
            report.ProcessingStatus.Should().Be(SubmittedPaymentProcessingStatus.Successful);
        }

        [Fact]
        public async Task The_store_accepts_invalid_responses_and_updates_the_processing_status_accordingly()
        {
            var requestId = "some-id";
            var sut = new InMemoryPaymentStore(logger);
            var request = CaptureFundsMessageBuilder.BuildRequest(requestId);
            var response = CaptureFundsMessageBuilder.BuildUnsuccessfulResponse();

            await sut.StoreCaptureFundsRequestAsync(request);
            await sut.StoreCaptureFundsResponseAsync(requestId, response);

            var report = await sut.RetrievePaymentDetailsAsync(requestId);
            report.ProcessingStatus.Should().Be(SubmittedPaymentProcessingStatus.Unsuccessful);
        }
    }
}
