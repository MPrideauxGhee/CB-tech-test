using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Options;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountDataStoreFactory(IOptions<DataStoreOptions> options) : IAccountDataStoreFactory
    {
        private readonly string _dataStoreType = options.Value.DataStoreType;

        public IAccountDataStore GetDataStore() =>
            _dataStoreType == "Backup"
                ? new BackupAccountDataStore()
                : new AccountDataStore();
    }
}
