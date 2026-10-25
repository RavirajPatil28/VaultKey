using Microsoft.EntityFrameworkCore;
using VaultKey.Data.Contexts.Interfaces;
using VaultKey.Data.Entities;

namespace VaultKey.Data.Contexts
{
    public partial class BankAuthDbContext(DbContextOptions options) : DbContext(options), IBankAuthDbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Account> Accounts => Set<Account>();

    }
}
