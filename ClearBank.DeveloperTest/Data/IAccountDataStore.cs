using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    /// <summary>
    /// Interface defining the contract for accessing and updating account details in the data store.
    /// </summary>
    public interface IAccountDataStore
    {
        /// <summary>
        /// Gets the account details for the specified account number from the data store
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        Account GetAccount(string accountNumber);
        /// <summary>
        /// Updates the account details in the data store
        /// </summary>
        /// <param name="account">The account to update</param>
        void UpdateAccount(Account account);
    }
}