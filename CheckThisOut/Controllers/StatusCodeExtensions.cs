using System.Net;
using JonBates.CheckThisOut.Core;

namespace JonBates.CheckThisOut.Controllers
{
    public static class StatusCodeExtensions
    {
        public static int ToStatusCode(this PaymentProcessErrorType @this)
        {
            return @this switch
            {
                PaymentProcessErrorType.TransactionAlreadyExists => (int) HttpStatusCode.Conflict,
                PaymentProcessErrorType.AcquiringBankValidationError => (int) HttpStatusCode.BadRequest,
                PaymentProcessErrorType.Exception => (int) HttpStatusCode.InternalServerError
            };
        }

    }
}