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

        public async Task<CaptureFundsResponse> ProcessAsync(CaptureFundsRequest request)
        {
            await _paymentStore.StoreCaptureFundsRequestAsync(request).ConfigureAwait(false);
            
            var response = await _bankClient.CaptureFundsAsync(request).ConfigureAwait(false);
            
            await _paymentStore.StoreCaptureFundsResponseAsync(response).ConfigureAwait(false);

            return response;
        }
    }
}
