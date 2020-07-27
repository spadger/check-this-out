using System;

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

        protected bool Equals(SubmittedPaymentDetails other)
        {
            return Equals(Request, other.Request) && ProcessingStatus == other.ProcessingStatus;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubmittedPaymentDetails) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Request, (int) ProcessingStatus);
        }
    }
}