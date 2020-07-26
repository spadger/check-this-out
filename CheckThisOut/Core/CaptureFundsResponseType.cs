namespace JonBates.CheckThisOut.Core
{
    public enum CaptureFundsResponseType
    {
        Success = 1,
        PaymentGatewayInaccessible,
        ValidationFailure,
        OtherFailure
    }
}