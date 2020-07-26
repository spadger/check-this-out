using System;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    //This is the only DTo to be bound, so the only class to have public setters
    public class CaptureFundsRequestDTO
    {
        public string RequestId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string PAN { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string CVV { get; set; }
        public string ThreeDSToken { get; set; }
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
                ValidFrom.Value,
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
    }
}
