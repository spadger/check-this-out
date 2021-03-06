﻿using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class CaptureFundsSuccessResponseDTO
    {
        public CaptureFundsSuccessResponseDTO(CaptureFundsBankResponse response)
        {
            ResponseId = response.ResponseId;
        }

        public string ResponseId { get; }

        protected bool Equals(CaptureFundsSuccessResponseDTO other)
        {
            return ResponseId == other.ResponseId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CaptureFundsSuccessResponseDTO) obj);
        }

        public override int GetHashCode()
        {
            return (ResponseId != null ? ResponseId.GetHashCode() : 0);
        }
    }
}