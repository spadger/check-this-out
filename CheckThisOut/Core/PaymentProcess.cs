using System.Threading.Tasks;
using JonBates.CheckThisOut.Core.BankClient;
using JonBates.CheckThisOut.Core.PaymentStore;

namespace JonBates.CheckThisOut.Core
{
    public class PaymentProcess : IPaymentProcess
    {
        public PaymentProcess(IPaymentStore paymentStore, IBankClient bankClient)
        {
            _paymentStore = paymentStore;
            _bankClient = bankClient;
        }

        private readonly IPaymentStore _paymentStore;
        private readonly IBankClient _bankClient;

        public async Task<Either<PaymentProcessErrorResult, CaptureFundsBankResponse>> ProcessAsync(CaptureFundsRequest request)
        {

            var storeResult = await _paymentStore.StoreCaptureFundsRequestAsync(request).ConfigureAwait(false);

            if (storeResult == StorePaymentRequestResult.AlreadyExists)
            {
                var result = new PaymentProcessErrorResult(PaymentProcessErrorType.TransactionAlreadyExists);
                return Either<PaymentProcessErrorResult, CaptureFundsBankResponse>.Left(result);
            }
            
            var response = await _bankClient.CaptureFundsAsync(request).ConfigureAwait(false);

            await _paymentStore.StoreCaptureFundsResponseAsync(request.RequestId, response).ConfigureAwait(false);

            return response;
        }
    }
}
