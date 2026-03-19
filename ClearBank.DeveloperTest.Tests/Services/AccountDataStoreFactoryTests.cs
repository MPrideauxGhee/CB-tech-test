using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Options;
namespace ClearBank.DeveloperTest.Tests.Services
{
    public class AccountDataStoreFactoryTests
    {
        [Fact]
        public void GetDataStore_ShouldReturnBackupAccountDataStore_WhenDataStoreTypeIsBackup()
        {
            var options = Options.Create(new DataStoreOptions { DataStoreType = "Backup" });
            var factory = new AccountDataStoreFactory(options);

            factory.GetDataStore().Should().BeOfType<BackupAccountDataStore>();
        }

        [Fact]
        public void GetDataStore_ShouldReturnAccountDataStore_WhenDataStoreTypeIsNotBackup()
        {
            var options = Options.Create(new DataStoreOptions { DataStoreType = "Primary" });
            var factory = new AccountDataStoreFactory(options);

            factory.GetDataStore().Should().BeOfType<AccountDataStore>();
        }
    }
}
