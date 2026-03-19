using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System;
using System.Transactions;

namespace ClearBank.DeveloperTest.Services
{
    public class TransactionService() : ITransactionService
    {
        public void Execute(Account account, decimal amount, IAccountDataStore accountDataStore)
        {
            account.Balance -= amount;

            try
            {
                accountDataStore.UpdateAccount(account);
            }
            catch (Exception ex)
            {
                //TODO: add logging
                throw new TransactionException("Failed to update account.", ex);
            }
        }
    }
}
