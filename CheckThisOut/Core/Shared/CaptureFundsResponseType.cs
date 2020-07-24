namespace JonBates.CheckThisOut.Core.Shared
{
    public enum CaptureFundsResponseType
    {
        Success = 1,
        PaymentGatewayInaccessible,
        ValidationFailure,
        OtherFailure
    }
}