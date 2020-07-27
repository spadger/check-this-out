using System.Threading.Tasks;

namespace JonBates.CheckThisOut.Core.PaymentStore
{
    public interface IPaymentStore
    {
        /// <summary>
        /// Idempotently stores a payment request, according to the request's RequestId
        /// </summary>
        /// <param name="request">The request to store</param>
        /// <returns>
        /// StorePaymentRequestResult.StoredSuccessfully if the request seems new,
        /// or StorePaymentRequestResult.AlreadyExists if the request is not new, indicating it will not be stored
        /// </returns>
        Task<StorePaymentRequestResult> StoreCaptureFundsRequestAsync(CaptureFundsRequest request);
        Task StoreCaptureFundsResponseAsync(string requestId, Either<PaymentProcessErrorResult, CaptureFundsBankResponse> bankResponse);
        Task<SubmittedPaymentDetails> RetrievePaymentDetailsAsync(string paymentRequestId);
    }
}
