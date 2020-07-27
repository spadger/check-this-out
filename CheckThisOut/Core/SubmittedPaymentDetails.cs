namespace JonBates.CheckThisOut.Core
{
    public class SubmittedPaymentDetails
    {
        public SubmittedPaymentDetails(CaptureFundsRequest request, SubmittedPaymentProcessingStatus processingStatus)
        {
            Request = request;
            ProcessingStatus = processingStatus;
        }

        public CaptureFundsRequest Request { get; }
        public SubmittedPaymentProcessingStatus ProcessingStatus { get; }
    }
}