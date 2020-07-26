namespace JonBates.CheckThisOut.Core
{
    public class SubmittedPayment
    {
        public SubmittedPayment(CaptureFundsRequest request, CaptureFundsResponse response)
        {
            Request = request;
            Response = response;
        }

        public CaptureFundsRequest Request { get; }
        public CaptureFundsResponse Response { get; }
    }
}