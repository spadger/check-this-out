using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JonBates.CheckThisOut.Core.PaymentStore
{
    public class InMemoryPaymentStore : IPaymentStore
    {
        private readonly ConcurrentDictionary<string, PaymentStatus> _cache = new ConcurrentDictionary<string, PaymentStatus>();
        private readonly ILogger<InMemoryPaymentStore> _log;

        public InMemoryPaymentStore(ILogger<InMemoryPaymentStore> log)
        {
            _log = log;
        }

        public Task<StorePaymentRequestResult> StoreCaptureFundsRequestAsync(CaptureFundsRequest request)
        {
            _log.LogInformation(">> StoreCaptureFundsRequestAsync");
            if (!_cache.TryAdd(request.RequestId, PaymentStatus.RequestOnly(request)))
            {
                _log.LogInformation("<< StoreCaptureFundsRequestAsync - request {requestId} already exists", request.RequestId);
                return Task.FromResult(StorePaymentRequestResult.AlreadyExists);
            }

            _log.LogInformation("<< StoreCaptureFundsRequestAsync - request {requestId} stored successfully", request.RequestId);
            return Task.FromResult(StorePaymentRequestResult.StoredSuccessfully);
        }

        public Task StoreCaptureFundsResponseAsync(string paymentRequestId, Either<PaymentProcessErrorResult, CaptureFundsBankResponse> processingResult)
        {
            _log.LogInformation("Storing processing result for {requestId}", paymentRequestId);
            
            _cache.AddOrUpdate(paymentRequestId,
                k => throw new InvalidOperationException("Could not find request with id ${paymentRequestId}"),
                (k, v) => v.WithResponse(processingResult)
            );

            return Task.CompletedTask;
        }

        public Task<SubmittedPaymentDetails> RetrievePaymentDetailsAsync(string paymentRequestId)
        {
            _log.LogInformation(">> RetrievePaymentDetailsAsync - request={paymentRequestId}", paymentRequestId);
            if (_cache.TryGetValue(paymentRequestId, out var cacheHit))
            {
                _log.LogInformation("<< RetrievePaymentDetailsAsync - request={paymentRequestId} found", paymentRequestId);
                var result = new SubmittedPaymentDetails(cacheHit.Request, cacheHit.ProcessingStatus);
                return Task.FromResult(result);
            }

            _log.LogInformation("<< RetrievePaymentDetailsAsync - request={paymentRequestId} not found", paymentRequestId);
            return Task.FromResult<SubmittedPaymentDetails>(null);
        }

        private readonly struct PaymentStatus
        {
            private PaymentStatus(CaptureFundsRequest request, Either<PaymentProcessErrorResult, CaptureFundsBankResponse>? processingResult)
            {
                Request = request;
                ProcessingResult = processingResult;
            }

            public CaptureFundsRequest Request { get; }

            public Either<PaymentProcessErrorResult, CaptureFundsBankResponse>? ProcessingResult { get; }

            public static PaymentStatus RequestOnly(CaptureFundsRequest request)
            {
                return new PaymentStatus(request, null);
            }

            public PaymentStatus WithResponse(
                Either<PaymentProcessErrorResult, CaptureFundsBankResponse> processingResult)
            {
                return new PaymentStatus(Request, processingResult);
            }

            public SubmittedPaymentProcessingStatus ProcessingStatus
            {
                get
                {
                    return ProcessingResult switch
                    {
                        null => SubmittedPaymentProcessingStatus.Pending,
                        { IsLeft: true } => SubmittedPaymentProcessingStatus.Unsuccessful,
                        _ => SubmittedPaymentProcessingStatus.Successful
                    };
                }
            }
        }
    }
}