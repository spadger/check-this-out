using System.Threading.Tasks;

namespace JonBates.CheckThisOut.Core
{
    public interface IPaymentProcess
    {
        Task<CaptureFundsResponse> ProcessAsync(CaptureFundsRequest request);
    }
}