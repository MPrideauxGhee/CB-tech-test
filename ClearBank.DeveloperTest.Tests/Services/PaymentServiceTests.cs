using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IAccountDataStore> _dataStoreMock;
        private readonly Mock<IAccountDataStoreFactory> _factoryMock;
        private readonly Mock<IPaymentSchemeValidator> _validatorMock;
        private readonly PaymentService _service;

        public PaymentServiceTests()
        {
            _dataStoreMock = new Mock<IAccountDataStore>();

            _factoryMock = new Mock<IAccountDataStoreFactory>();
            _factoryMock.Setup(f => f.GetDataStore()).Returns(_dataStoreMock.Object);

            _validatorMock = new Mock<IPaymentSchemeValidator>();
            _validatorMock.SetupGet(v => v.Scheme).Returns(PaymentScheme.Bacs);

            _service = new PaymentService(_factoryMock.Object, [_validatorMock.Object]);
        }

        [Fact]
        public void MakePayment_ShouldReturnSuccess_WhenValidRequest()
        {
            var account = new Account { Balance = 10 };
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs, Amount = 5 };

            _dataStoreMock.Setup(d => d.GetAccount(It.IsAny<string>())).Returns(account);
            _validatorMock.Setup(v => v.IsValid(account, request)).Returns(true);

            var result = _service.MakePayment(request);

            result.Success.Should().BeTrue();
            account.Balance.Should().Be(5);
            _dataStoreMock.Verify(d => d.UpdateAccount(account), Times.Once);
        }

        [Fact]
        public void MakePayment_ShouldReturnFalse_WhenValidatorFails()
        {
            var account = new Account { Balance = 10 };
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs, Amount = 5 };

            _dataStoreMock.Setup(d => d.GetAccount(It.IsAny<string>())).Returns(account);
            _validatorMock.Setup(v => v.IsValid(account, request)).Returns(false);

            var result = _service.MakePayment(request);

            result.Success.Should().BeFalse();
            account.Balance.Should().Be(10);
            _dataStoreMock.Verify(d => d.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void MakePayment_ShouldReturnFalse_WhenNoValidator()
        {
            var account = new Account { Balance = 10 };
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments, Amount = 5 };

            _dataStoreMock.Setup(d => d.GetAccount(It.IsAny<string>())).Returns(account);
            var result = _service.MakePayment(request);

            result.Success.Should().BeFalse();
            account.Balance.Should().Be(10);
            _dataStoreMock.Verify(d => d.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}