using System;

namespace JonBates.CheckThisOut.Core
{
    public class CaptureFundsRequest
    {
        public CaptureFundsRequest(string requestId, Guid merchantId, string customerName, string address, string postCode, string pan, decimal amount, DateTime validFrom, DateTime validTo, string cvv, string threeDsToken, DateTime postingDate)
        {
            RequestId = requestId;
            MerchantId = merchantId;
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

        public string RequestId { get; set; }
        public Guid MerchantId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string PAN { get; set; } // Card type can be inferred from the BIN
        public decimal Amount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string CVV { get; set; }
        public string ThreeDSToken { get; set; }
        public DateTime PostingDate { get; set; }

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