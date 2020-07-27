using System.Threading.Tasks;

namespace JonBates.CheckThisOut.Core.BankClient
{
    public interface IBankClient
    {
        Task<Either<PaymentProcessErrorResult, CaptureFundsBankResponse>> CaptureFundsAsync(CaptureFundsRequest request);
    }
}
