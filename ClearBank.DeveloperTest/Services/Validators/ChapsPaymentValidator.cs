using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Validators
{ 
    //TODO: doc this 
    public class ChapsPaymentValidator : IPaymentSchemeValidator
    {
        public PaymentScheme Scheme => PaymentScheme.Chaps;

        public bool IsValid(Account account, MakePaymentRequest request) =>
            account?.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) == true
            && account.Status == AccountStatus.Live;
    }
}
