using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Validators
{ 
    public class ChapsPaymentValidator : IPaymentSchemeValidator
    {
        public PaymentScheme Scheme => PaymentScheme.Chaps;

        public bool IsValid(Account account, MakePaymentRequest request) =>
            account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) && account.Status == AccountStatus.Live;
    }
}
