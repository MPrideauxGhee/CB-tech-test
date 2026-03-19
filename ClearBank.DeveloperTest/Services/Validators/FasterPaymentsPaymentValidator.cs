using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Validators
{
    public class FasterPaymentsPaymentValidator : IPaymentSchemeValidator
    {
        public PaymentScheme Scheme => PaymentScheme.FasterPayments;

        public bool IsValid(Account account, MakePaymentRequest request) =>
            account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) && account.Balance >= request.Amount;
    }
}
