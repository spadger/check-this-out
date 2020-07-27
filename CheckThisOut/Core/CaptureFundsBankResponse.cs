namespace JonBates.CheckThisOut.Core
{
    public class CaptureFundsBankResponse
    {
        public CaptureFundsBankResponse(string responseId)
        {
            ResponseId = responseId;
        }

        public string ResponseId { get; }
    }

}