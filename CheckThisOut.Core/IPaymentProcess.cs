using System.Threading.Tasks;
namespace JonBates.CheckThisOut.Core
{
    public interface IPaymentProcess
    {
        Task<Either<PaymentProcessErrorResult, CaptureFundsBankResponse>> ProcessAsync(CaptureFundsRequest request);
    }
}