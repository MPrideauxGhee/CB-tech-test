using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountDataStoreFactory(string dataStoreType) : IAccountDataStoreFactory
    {
        private readonly string _dataStoreType = dataStoreType;

        public IAccountDataStore GetDataStore() =>
            _dataStoreType == "Backup"
                ? new BackupAccountDataStore()
                : new AccountDataStore();
    }
}
