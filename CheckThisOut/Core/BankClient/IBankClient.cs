using System.Threading.Tasks;
using JonBates.CheckThisOut.Core.Shared;

namespace JonBates.CheckThisOut.Core.BankClient
{
    public interface IBankClient
    {
        Task<CaptureFundsResponse> CaptureFundsAsync(CaptureFundsRequest request);
    }
}
