using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountDataStoreFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IAccountDataStore GetDataStore();
    }
}
