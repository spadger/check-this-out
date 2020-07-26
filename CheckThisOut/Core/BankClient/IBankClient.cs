using System.Threading.Tasks;

namespace JonBates.CheckThisOut.Core.BankClient
{
    public interface IBankClient
    {
        Task<CaptureFundsResponse> CaptureFundsAsync(CaptureFundsRequest request);
    }
}
