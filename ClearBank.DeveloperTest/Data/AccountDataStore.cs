using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    /// <inheritdoc/>
    public class AccountDataStore : IAccountDataStore
    {
        /// <inheritdoc/>
        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            return new Account();
        }

        /// <inheritdoc/>
        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
        }
    }
}
