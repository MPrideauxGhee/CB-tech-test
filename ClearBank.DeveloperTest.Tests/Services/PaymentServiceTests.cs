using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;
using System.Transactions;

namespace ClearBank.DeveloperTest.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IAccountDataStore> _dataStoreMock;
        private readonly Mock<IAccountDataStoreFactory> _factoryMock;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<IPaymentSchemeValidator> _validatorMock;
        private readonly PaymentService _service;
        private readonly Account _account = new();
        private readonly MakePaymentRequest _request = new() { PaymentScheme = PaymentScheme.Bacs };

        public PaymentServiceTests()
        {
            _dataStoreMock = new Mock<IAccountDataStore>();
            _factoryMock = new Mock<IAccountDataStoreFactory>();
            _factoryMock.Setup(f => f.GetDataStore()).Returns(_dataStoreMock.Object);
            _transactionServiceMock = new Mock<ITransactionService>();
            _validatorMock = new Mock<IPaymentSchemeValidator>();

            var validators = new Dictionary<PaymentScheme, IPaymentSchemeValidator>
        {
            { PaymentScheme.Bacs, _validatorMock.Object }
        };

            _service = new PaymentService(_factoryMock.Object, _transactionServiceMock.Object, validators);
        }

        [Fact]
        public void MakePayment_ShouldReturnSuccess_WhenValidRequest()
        {
            _dataStoreMock.Setup(d => d.GetAccount(It.IsAny<string>())).Returns(_account);
            _validatorMock.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<MakePaymentRequest>())).Returns(true);

            var result = _service.MakePayment(_request);

            result.Success.Should().BeTrue();
            _transactionServiceMock.Verify(t => t.Execute(_account, _request.Amount, _dataStoreMock.Object), Times.Once);
        }

        [Fact]
        public void MakePayment_ShouldReturnFailure_WhenNullAccount()
        {
            _dataStoreMock.Setup(d => d.GetAccount(It.IsAny<string>())).Returns((Account)null);

            var result = _service.MakePayment(_request);

            result.Success.Should().BeFalse();
            _transactionServiceMock.Verify(t => t.Execute(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<IAccountDataStore>()), Times.Never);
        }

        [Fact]
        public void MakePayment_ShouldReturnFailure_WhenValidatorFails()
        {
            _dataStoreMock.Setup(d => d.GetAccount(It.IsAny<string>())).Returns(_account);
            _validatorMock.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<MakePaymentRequest>())).Returns(false);

            var result = _service.MakePayment(_request);

            result.Success.Should().BeFalse();
            _transactionServiceMock.Verify(t => t.Execute(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<IAccountDataStore>()), Times.Never);
        }

        [Fact]
        public void MakePayment_ShouldReturnFailure_WhenTransactionServiceThrows()
        {
            _dataStoreMock.Setup(d => d.GetAccount(It.IsAny<string>())).Returns(_account);
            _validatorMock.Setup(v => v.IsValid(It.IsAny<Account>(), It.IsAny<MakePaymentRequest>())).Returns(true);
            _transactionServiceMock.Setup(t => t.Execute(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<IAccountDataStore>()))
                                   .Throws<TransactionException>();

            var result = _service.MakePayment(_request);

            result.Success.Should().BeFalse();
        }
    }
}