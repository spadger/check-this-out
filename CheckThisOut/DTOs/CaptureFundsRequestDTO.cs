using System;
using System.ComponentModel.DataAnnotations;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    //This is the only DTo to be bound, so the only class to have public setters
    public class CaptureFundsRequestDTO
    {
        [Required]
        public string RequestId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required, RegularExpression("\\d{16}")]
        public string PAN { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required, RegularExpression("[A-Z]{3}")]
        public string Currency { get; set; }
        [Required]
        public DateTime? ValidFrom { get; set; }
        [Required]
        public DateTime? ValidTo { get; set; }
        [Required, RegularExpression("\\d{3}")]
        public string CVV { get; set; }
        [Required]
        public string ThreeDSToken { get; set; }
        [Required]
        public DateTime? PostingDate { get; set; }

        public CaptureFundsRequest ToRequest(Guid merchantId) 
        {
            // Could have use AutoMapper here I guess, but this application is trivial and no flattening is required
            return new CaptureFundsRequest(
                RequestId,
                merchantId,
                CustomerName,
                Address,
                PostCode,
                PAN,
                Amount,
                Currency,
                ValidFrom.Value, //These are safe because we have validated they are not null
                ValidTo.Value,
                CVV,
                ThreeDSToken,
                PostingDate.Value
            );
        }

        public static CaptureFundsRequestDTO From(CaptureFundsRequest request)
        {
            return new CaptureFundsRequestDTO
            {
                RequestId = request.RequestId,
                CustomerName = request.CustomerName,
                Address = request.Address,
                PostCode = request.PostCode,
                PAN = request.PAN,
                Amount = request.Amount,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo,
                CVV = request.CVV,
                ThreeDSToken = request.ThreeDSToken,
                PostingDate = request.PostingDate
            };
        }

        protected bool Equals(CaptureFundsRequestDTO other)
        {
            return RequestId == other.RequestId && CustomerName == other.CustomerName && Address == other.Address && PostCode == other.PostCode && PAN == other.PAN && Amount == other.Amount && Currency == other.Currency && Nullable.Equals(ValidFrom, other.ValidFrom) && Nullable.Equals(ValidTo, other.ValidTo) && CVV == other.CVV && ThreeDSToken == other.ThreeDSToken && Nullable.Equals(PostingDate, other.PostingDate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CaptureFundsRequestDTO) obj);
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
            hashCode.Add(Currency);
            hashCode.Add(ValidFrom);
            hashCode.Add(ValidTo);
            hashCode.Add(CVV);
            hashCode.Add(ThreeDSToken);
            hashCode.Add(PostingDate);
            return hashCode.ToHashCode();
        }
    }
}
