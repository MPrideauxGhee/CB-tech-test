using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface ITransactionService
    {
        void Execute(Account account, decimal amount, IAccountDataStore accountDataStore);
    }
}
