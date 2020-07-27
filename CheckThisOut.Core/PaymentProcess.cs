using System.Threading.Tasks;
using JonBates.CheckThisOut.Core.BankClient;
using JonBates.CheckThisOut.Core.PaymentStore;
using Microsoft.Extensions.Logging;

namespace JonBates.CheckThisOut.Core
{
    public class PaymentProcess : IPaymentProcess
    {
        public PaymentProcess(ILogger<PaymentProcess> log, IPaymentStore paymentStore, IBankClient bankClient)
        {
            _log = log;
            _paymentStore = paymentStore;
            _bankClient = bankClient;
        }

        private readonly ILogger<PaymentProcess> _log;
        private readonly IPaymentStore _paymentStore;
        private readonly IBankClient _bankClient;

        public async Task<Either<PaymentProcessErrorResult, CaptureFundsBankResponse>> ProcessAsync(CaptureFundsRequest request)
        {
            _log.LogInformation(">> Handling payment request={requestId}", request.RequestId);

            var storeResult = await _paymentStore.StoreCaptureFundsRequestAsync(request).ConfigureAwait(false);

            if (storeResult == StorePaymentRequestResult.AlreadyExists)
            {
                var result = new PaymentProcessErrorResult(PaymentProcessErrorType.TransactionAlreadyExists);

                _log.LogInformation("<< Handling payment request={requestId} - already being processed", request.RequestId);

                return Either<PaymentProcessErrorResult, CaptureFundsBankResponse>.Left(result);
            }
            
            var response = await _bankClient.CaptureFundsAsync(request).ConfigureAwait(false);

            await _paymentStore.StoreCaptureFundsResponseAsync(request.RequestId, response).ConfigureAwait(false);

            _log.LogInformation("<< Handling payment request={requestId} - handled", request.RequestId);

            return response;
        }
    }
}
