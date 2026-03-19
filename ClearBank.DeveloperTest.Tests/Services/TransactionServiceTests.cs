using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using System;
using System.Transactions;

namespace ClearBank.DeveloperTest.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<IAccountDataStore> _dataStoreMock;
        private readonly TransactionService _service;

        public TransactionServiceTests()
        {
            _dataStoreMock = new Mock<IAccountDataStore>();

            _service = new TransactionService();
        }

        [Fact]
        public void Execute_DeductsAmountFromBalance()
        {
            var account = new Account { Balance = 100 };

            _service.Execute(account, 40, _dataStoreMock.Object);

            account.Balance.Should().Be(60);
        }

        [Fact]
        public void Execute_CallsUpdateAccount_WhenValid()
        {
            var account = new Account { Balance = 100 };

            _service.Execute(account, 40, _dataStoreMock.Object);

            _dataStoreMock.Verify(d => d.UpdateAccount(account), Times.Once);
        }

        [Fact]
        public void Execute_ShouldThrowTransactionException_WhenUpdateAccountThrows()
        {
            var account = new Account { Balance = 100 };
            _dataStoreMock.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Throws<Exception>();

            var act = () => _service.Execute(account, 40, _dataStoreMock.Object);

            act.Should().Throw<TransactionException>();
        }

        [Fact]
        public void Execute_OriginalExceptionIsInnerException_WhenUpdateAccountThrows()
        {
            var account = new Account { Balance = 100 };
            var originalException = new Exception("Database error");
            _dataStoreMock.Setup(d => d.UpdateAccount(It.IsAny<Account>())).Throws(originalException);

            var act = () => _service.Execute(account, 40, _dataStoreMock.Object);

            act.Should().Throw<TransactionException>().WithInnerException<Exception>().WithMessage("Database error");
        }
    }
}
