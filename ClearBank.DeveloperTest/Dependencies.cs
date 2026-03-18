using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.Validators;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace ClearBank.DeveloperTest
{
    public static class Dependencies
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            var dataStoreType = ConfigurationManager.AppSettings["DataStoreType"];

            services.AddSingleton<IAccountDataStoreFactory>(
                new AccountDataStoreFactory(dataStoreType));

            services.AddTransient<IPaymentService, PaymentService>();

            services.AddTransient<IPaymentSchemeValidator, BacsPaymentValidator>();
            services.AddTransient<IPaymentSchemeValidator, FasterPaymentsPaymentValidator>();
            services.AddTransient<IPaymentSchemeValidator, ChapsPaymentValidator>();

            return services;
        }
    }
}
