using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Validators
{
    public class BacsPaymentValidator : IPaymentSchemeValidator
    {
        public PaymentScheme Scheme => PaymentScheme.Bacs;

        public bool IsValid(Account account, MakePaymentRequest request) => 
            account?.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs) == true;
    }
}
