using System.Threading.Tasks;
using JonBates.CheckThisOut.Core.Shared;

namespace JonBates.CheckThisOut.Core.PaymentStore
{
    interface IPaymentStore
    {
        Task StoreCaptureFundsRequestAsync(CaptureFundsRequest request);
        Task<CaptureFundsResponse> StoreCaptureFundsResponseAsync(string paymentRequestId);
    }
}
