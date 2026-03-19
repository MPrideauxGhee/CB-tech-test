using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.Validators;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ClearBank.DeveloperTest
{
    public static class Dependencies
    {
        public static IServiceCollection Register(IServiceCollection services)
        {
            RegisterDataStores(services);
            RegisterServices(services);
            RegisterValidators(services);

            return services;
        }

        private static void RegisterDataStores(IServiceCollection services)
        {
            services.AddSingleton<IAccountDataStoreFactory, AccountDataStoreFactory>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITransactionService, TransactionService>();
        }

        private static void RegisterValidators(IServiceCollection services)
        {
            services.AddSingleton<IPaymentSchemeValidator, BacsPaymentValidator>();
            services.AddSingleton<IPaymentSchemeValidator, FasterPaymentsPaymentValidator>();
            services.AddSingleton<IPaymentSchemeValidator, ChapsPaymentValidator>();

            services.AddSingleton<IReadOnlyDictionary<PaymentScheme, IPaymentSchemeValidator>>(provider =>
            {
                var validators = provider.GetServices<IPaymentSchemeValidator>()
                                         .ToDictionary(v => v.Scheme);

                var missingSchemes = Enum.GetValues<PaymentScheme>()
                                         .Where(s => !validators.ContainsKey(s))
                                         .ToList();

                if (missingSchemes.Count != 0)
                {
                    throw new InvalidOperationException(
                        $"No validator registered for payment scheme(s): {string.Join(", ", missingSchemes)}");
                }

                return validators;
            });
        }
    }
}
