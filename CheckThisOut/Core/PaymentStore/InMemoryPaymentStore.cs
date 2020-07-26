using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace JonBates.CheckThisOut.Core.PaymentStore
{
    public class InMemoryPaymentStore : IPaymentStore
    {
        private readonly ConcurrentDictionary<string, Tuple<CaptureFundsRequest, CaptureFundsResponse>> _cache 
            = new ConcurrentDictionary<string, Tuple<CaptureFundsRequest, CaptureFundsResponse>>();

        public Task<StorePaymentRequestResult> StoreCaptureFundsRequestAsync(CaptureFundsRequest request)
        {
            if(!_cache.TryAdd(request.RequestId, Tuple.Create(request, null as CaptureFundsResponse)))
            {
                return Task.FromResult(StorePaymentRequestResult.AlreadyExists);
            }

            return Task.FromResult(StorePaymentRequestResult.StoredSuccessfully);
        }

        public Task StoreCaptureFundsResponseAsync(CaptureFundsResponse response)
        {
            _cache.AddOrUpdate(response.RequestId,
                k => throw new Exception($"Could not find request with id {response.RequestId}"),
                (k, v) => Tuple.Create(v.Item1, response)
            );

            return Task.CompletedTask;
        }

        public Task<SubmittedPayment> RetrievePaymentDetailsAsync(string paymentRequestId)
        {

            if (_cache.TryGetValue(paymentRequestId, out var cacheHit))
            {
                var result = new SubmittedPayment(cacheHit.Item1, cacheHit.Item2);
                return Task.FromResult(result);
            }

            return Task.FromResult<SubmittedPayment>(null);
        }
    }
}