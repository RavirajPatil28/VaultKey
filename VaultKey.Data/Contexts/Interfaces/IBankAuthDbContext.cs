using Microsoft.EntityFrameworkCore;
using VaultKey.Data.Entities;

namespace VaultKey.Data.Contexts.Interfaces
{
    public interface IBankAuthDbContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<Account> Accounts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
