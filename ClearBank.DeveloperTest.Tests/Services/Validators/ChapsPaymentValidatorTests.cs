using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Services.Validators
{
    public class ChapsPaymentValidatorTests
    {
        private readonly ChapsPaymentValidator _validator = new();
        private readonly MakePaymentRequest _request = new();

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs)]
        [InlineData(AllowedPaymentSchemes.FasterPayments)]
        public void IsValid_ShouldReturnFalse_WhenNotChapsSchemeAndLive(AllowedPaymentSchemes scheme)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = scheme,
                Status = AccountStatus.Live
            };

            _validator.IsValid(account, _request).Should().BeFalse();
        }

        [Theory]
        [InlineData(AccountStatus.Disabled)]
        [InlineData(AccountStatus.InboundPaymentsOnly)]
        public void IsValid_ShouldReturnFalse_WhenAccountIsNotLiveAndChapsScheme(AccountStatus status)
        {
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = status
            };
            _validator.IsValid(account, _request).Should().BeFalse();
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAccountIsLiveAndChapsScheme()
        {
            var account = new Account 
            { 
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Status = AccountStatus.Live
            };

            _validator.IsValid(account, _request).Should().BeTrue();
        }
    }
}