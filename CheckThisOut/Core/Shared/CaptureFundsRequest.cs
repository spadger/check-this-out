using System;

namespace JonBates.CheckThisOut.Core.Shared
{
    public class CaptureFundsRequest
    {
        public CaptureFundsRequest(string requestId, string customerName, string address, string postCode, string pan, decimal amount, DateTime validFrom, DateTime validTo, string cvv, string threeDsToken, DateTime postingDate)
        {
            RequestId = requestId;
            CustomerName = customerName;
            Address = address;
            PostCode = postCode;
            PAN = pan;
            Amount = amount;
            ValidFrom = validFrom;
            ValidTo = validTo;
            CVV = cvv;
            ThreeDSToken = threeDsToken;
            PostingDate = postingDate;
        }

        public string RequestId { get; }
        public string CustomerName { get; }
        public string Address { get; }
        public string PostCode { get; }
        public string PAN { get; } // Card type can be inferred from the BIN
        public decimal Amount { get; }
        public DateTime ValidFrom { get; }
        public DateTime ValidTo { get; }
        public string CVV { get; }
        public string ThreeDSToken { get; }
        public DateTime PostingDate { get; }

        protected bool Equals(CaptureFundsRequest other)
        {
            return RequestId == other.RequestId && CustomerName == other.CustomerName && Address == other.Address && PostCode == other.PostCode && PAN == other.PAN && Amount == other.Amount && ValidFrom.Equals(other.ValidFrom) && ValidTo.Equals(other.ValidTo) && CVV == other.CVV && ThreeDSToken == other.ThreeDSToken && PostingDate == other.PostingDate;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CaptureFundsRequest) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(RequestId);
            hashCode.Add(CustomerName);
            hashCode.Add(Address);
            hashCode.Add(PostCode);
            hashCode.Add(PAN);
            hashCode.Add(Amount);
            hashCode.Add(ValidFrom);
            hashCode.Add(ValidTo);
            hashCode.Add(CVV);
            hashCode.Add(ThreeDSToken);
            hashCode.Add(PostingDate);
            return hashCode.ToHashCode();
        }
    }
}