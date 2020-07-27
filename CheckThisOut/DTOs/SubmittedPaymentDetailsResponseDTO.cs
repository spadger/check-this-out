﻿using System;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.DTOs
{
    public class SubmittedPaymentDetailsResponseDTO
    {
        public SubmittedPaymentDetailsResponseDTO(SubmittedPaymentDetails paymentDetails)
        {
            Request = CaptureFundsRequestDTO.From(paymentDetails.Request);
            ProcessingStatus = paymentDetails.ProcessingStatus;
        }

        public CaptureFundsRequestDTO Request { get; }
        public SubmittedPaymentProcessingStatus ProcessingStatus { get; }

        protected bool Equals(SubmittedPaymentDetailsResponseDTO other)
        {
            return Equals(Request, other.Request) && ProcessingStatus == other.ProcessingStatus;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubmittedPaymentDetailsResponseDTO) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Request, (int) ProcessingStatus);
        }
    }
}