using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Services.Validators
{
    public class FasterPaymentsPaymentValidatorTests
    {
        private readonly FasterPaymentsPaymentValidator _validator = new();
        private readonly MakePaymentRequest _request = new() { Amount = 5 };

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAccountIsNull()
        {
            _validator.IsValid(null, _request).Should().BeFalse();
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs)]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        public void IsValid_ShouldReturnFalse_WhenNotFasterPaymentsScheme(AllowedPaymentSchemes scheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = scheme,
                Balance = 10
            };

            _validator.IsValid(account, _request).Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenInsufficientBalance()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 1
            };

            _validator.IsValid(account, _request).Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenFasterPaymentsSchemeAndSufficientBalance()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 10
            };

            _validator.IsValid(account, _request).Should().BeTrue();
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenFasterPaymentsSchemeAndExactBalance()
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 5
            };

            _validator.IsValid(account, _request).Should().BeTrue();
        }
    }
}