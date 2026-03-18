using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ClearBank.DeveloperTest.Tests.Services
{
    public class AccountDataStoreFactoryTests
    {
        [Fact]
        public void GetDataStore_WhenDataStoreTypeIsBackup_ReturnsBackupAccountDataStore()
        {
            var factory = new AccountDataStoreFactory("Backup");

            var result = factory.GetDataStore();

            result.Should().BeOfType<BackupAccountDataStore>();
        }

        [Fact]
        public void GetDataStore_WhenDataStoreTypeIsNotBackup_ReturnsAccountDataStore()
        {
            var factory = new AccountDataStoreFactory("Primary");

            var result = factory.GetDataStore();

            result.Should().BeOfType<AccountDataStore>();
        }
    }
}
