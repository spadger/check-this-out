namespace JonBates.CheckThisOut.Core
{
    public class CaptureFundsBankResponse
    {
        public CaptureFundsBankResponse(string responseId)
        {
            ResponseId = responseId;
        }

        public string ResponseId { get; }

        protected bool Equals(CaptureFundsBankResponse other)
        {
            return ResponseId == other.ResponseId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CaptureFundsBankResponse) obj);
        }

        public override int GetHashCode()
        {
            return (ResponseId != null ? ResponseId.GetHashCode() : 0);
        }
    }

}