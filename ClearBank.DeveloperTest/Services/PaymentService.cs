using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(
        IAccountDataStoreFactory dataStoreFactory,
        IEnumerable<IPaymentSchemeValidator> validators) : IPaymentService
    {
        private readonly IAccountDataStoreFactory _dataStoreFactory = dataStoreFactory;
        private readonly IReadOnlyDictionary<PaymentScheme, IPaymentSchemeValidator> _validators = validators.ToDictionary(v => v.Scheme);

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStore = _dataStoreFactory.GetDataStore();

            var account = dataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult
            {
                Success = false
            };

            if (_validators.TryGetValue(request.PaymentScheme, out var validator))
            {
                result.Success = validator.IsValid(account, request);
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;
                dataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
