using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services.Validators
{
    /// <summary>
    /// Defines functionality for validating payment requests against specific payment schemes.
    /// </summary>
    public interface IPaymentSchemeValidator
    {
        /// <summary>
        /// Payment scheme that this validator is responsible for validating.
        /// </summary>
        PaymentScheme Scheme { get; }
        /// <summary>
        /// Validates the given payment request against the specified account details to determine if the payment can be processed under the rules of the associated payment scheme.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        bool IsValid(Account account, MakePaymentRequest request);
    }
}