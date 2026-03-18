using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountDataStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        Account GetAccount(string accountNumber);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        void UpdateAccount(Account account);
    }
}