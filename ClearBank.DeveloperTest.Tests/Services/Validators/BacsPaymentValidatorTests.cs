using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Services.Validators
{
    public class BacsPaymentValidatorTests
    {
        private readonly BacsPaymentValidator _validator = new();
        private readonly MakePaymentRequest _request = new();

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAccountIsNull()
        {
            _validator.IsValid(null, _request).Should().BeFalse();
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.FasterPayments)]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        public void IsValid_ShouldReturnFalse_WhenNotBacsScheme(AllowedPaymentSchemes scheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = scheme
            };

            _validator.IsValid(account, _request).Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenBacsScheme()
        {
            var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };

            _validator.IsValid(account, _request).Should().BeTrue();
        }
    }
}