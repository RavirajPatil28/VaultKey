using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VaultKey.Business.Security;
using VaultKey.Business.Security.Interfaces;
using VaultKey.Business.Services;
using VaultKey.Business.Services.Interfaces;
using VaultKey.Data.Contexts;
using VaultKey.Data.Contexts.Interfaces;

namespace VaultKey.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVaultKeyServices(this IServiceCollection serviceCollection)
        {
            // Add services here
            serviceCollection.AddSingleton<IFieldEncryptor, AesGcmFieldEncryptor>();
            serviceCollection.AddScoped<ICustomerService, CustomerService>();

            return serviceCollection;
        }

        public static IServiceCollection AddBankAuthDatabases(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> action)
        {
            serviceCollection.AddDbContext<IBankAuthDbContext, BankAuthDbContext>(opt => action(opt));
            return serviceCollection;
        }
    }
}
