using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;
using System.Transactions;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService(
        IAccountDataStoreFactory dataStoreFactory,
        ITransactionService transactionService,
        IReadOnlyDictionary<PaymentScheme, IPaymentSchemeValidator> validators) : IPaymentService
    {
        private readonly IAccountDataStoreFactory _dataStoreFactory = dataStoreFactory;
        private readonly ITransactionService _transactionService = transactionService;
        private readonly IReadOnlyDictionary<PaymentScheme, IPaymentSchemeValidator> _validators = validators;

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStore = _dataStoreFactory.GetDataStore();
            var account = dataStore.GetAccount(request.DebtorAccountNumber);

            if (account == null)
            {
                return new MakePaymentResult();
            }

            var validator = _validators[request.PaymentScheme];

            if (!validator.IsValid(account, request))
            {
                return new MakePaymentResult();
            }

            try
            {
                _transactionService.Execute(account, request.Amount, dataStore);
            }
            catch (TransactionException)
            {
                return new MakePaymentResult();
            }

            return new MakePaymentResult { Success = true };
        }
    }
}
