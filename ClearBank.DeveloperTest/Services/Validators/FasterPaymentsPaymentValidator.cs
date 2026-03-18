using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Validators
{
    //TODO: doc this and the rest
    public class FasterPaymentsPaymentValidator : IPaymentSchemeValidator
    {
        public PaymentScheme Scheme => PaymentScheme.FasterPayments;

        public bool IsValid(Account account, MakePaymentRequest request) =>
            account?.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) == true
            && account.Balance >= request.Amount;
    }
}
