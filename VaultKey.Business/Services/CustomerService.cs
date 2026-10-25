using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using VaultKey.Business.Models;
using VaultKey.Business.Security;
using VaultKey.Business.Security.Interfaces;
using VaultKey.Business.Services.Interfaces;
using VaultKey.Data.Contexts.Interfaces;
using VaultKey.Data.Entities;
using Microsoft.EntityFrameworkCore;
using static VaultKey.Business.Models.CustomerResponses;

namespace VaultKey.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private const int MaxGenerateAttempts = 5;

        private readonly IBankAuthDbContext _dbContext;
        private readonly IFieldEncryptor _encryptor;

        public CustomerService(IBankAuthDbContext dbContext, IFieldEncryptor encryptor)
        {
            _dbContext = dbContext;
            _encryptor = encryptor;
        }

        public async Task<CustomerCreatedResponse> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
        {
            // Normalize first, so what is stored is consistent.
            var fullName = CollapseSpaces(request.FullName);
            var email = request.Email.Trim().ToLowerInvariant();
            var dob = request.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var mobile = string.IsNullOrWhiteSpace(request.Mobile) ? null : request.Mobile.Trim();

            var customerNumber = await GenerateUniqueCustomerNumberAsync(ct);
            var (accountNumber, accountIndex) = await GenerateUniqueAccountNumberAsync(ct);

            var keyVersion = _encryptor.CurrentKeyVersion;

            var customer = new Customer
            {
                CustomerNumber = customerNumber,
                FullNameEnc = _encryptor.Encrypt(fullName, EncryptionContexts.CustomerFullName),
                DobEnc = _encryptor.Encrypt(dob, EncryptionContexts.CustomerDob),
                EmailEnc = _encryptor.Encrypt(email, EncryptionContexts.CustomerEmail),
                MobileEnc = mobile is null ? null : _encryptor.Encrypt(mobile, EncryptionContexts.CustomerMobile),
                KeyVersion = keyVersion,
                Status = "Active",
                IsActivated = false
            };

            customer.Accounts.Add(new Account
            {
                AccountNumberEnc = _encryptor.Encrypt(accountNumber, EncryptionContexts.AccountNumber),
                AccountNumberBlindIndex = accountIndex,
                AccountLast4 = accountNumber[^4..],
                AccountType = request.AccountType,
                Balance = request.OpeningBalance,
                Currency = "INR",
                KeyVersion = keyVersion
            });

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync(ct); // customer + account are saved in one transaction

            return new CustomerCreatedResponse(
                customer.CustomerNumber,
                fullName,
                "XXXXXXXX" + accountNumber[^4..],
                request.AccountType,
                request.OpeningBalance,
                "INR",
                customer.CreatedAt);
        }

        public async Task<CustomerProfileResponse?> GetProfileAsync(string customerNumber, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(customerNumber) || !RegexUtils.CustomerNumberRegex().IsMatch(customerNumber))
            {
                return null;
            }

            // Profile only: accounts are not loaded.
            var customer = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerNumber == customerNumber, ct);

            if (customer is null)
            {
                return null;
            }

            return new CustomerProfileResponse(
                customer.CustomerNumber,
                _encryptor.Decrypt(customer.FullNameEnc, EncryptionContexts.CustomerFullName),
                DateOnly.ParseExact(
                    _encryptor.Decrypt(customer.DobEnc, EncryptionContexts.CustomerDob),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture),
                _encryptor.Decrypt(customer.EmailEnc, EncryptionContexts.CustomerEmail),
                customer.MobileEnc is null ? null : _encryptor.Decrypt(customer.MobileEnc, EncryptionContexts.CustomerMobile),
                customer.Status,
                customer.IsActivated,
                customer.CreatedAt);
        }

        private async Task<string> GenerateUniqueCustomerNumberAsync(CancellationToken ct)
        {
            for (var attempt = 0; attempt < MaxGenerateAttempts; attempt++)
            {
                var candidate = "CUST" + RandomDigits(7);
                if (!await _dbContext.Customers.AsQueryable().AnyAsync(c => c.CustomerNumber == candidate, ct))
                {
                    return candidate;
                }
            }

            throw new InvalidOperationException("Could not generate a unique customer number. Try again.");
        }

        private async Task<(string AccountNumber, byte[] BlindIndex)> GenerateUniqueAccountNumberAsync(CancellationToken ct)
        {
            for (var attempt = 0; attempt < MaxGenerateAttempts; attempt++)
            {
                var candidate = "50" + RandomDigits(10); // 12 digits
                var index = _encryptor.ComputeBlindIndex(candidate, EncryptionContexts.AccountNumber);

                if (!await _dbContext.Accounts.AsQueryable().AnyAsync(a => a.AccountNumberBlindIndex == index, ct))
                {
                    return (candidate, index);
                }
            }

            throw new InvalidOperationException("Could not generate a unique account number. Try again.");
        }

        private static string RandomDigits(int length)
        {
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                chars[i] = (char)('0' + RandomNumberGenerator.GetInt32(10));
            }

            return new string(chars);
        }

        private static string CollapseSpaces(string value) =>
            RegexUtils.MultiSpaceRegex().Replace(value.Trim(), " ");

    }

    public static partial class RegexUtils
    {
        [GeneratedRegex(@"\s+")]
        public static partial Regex MultiSpaceRegex();

        [GeneratedRegex(@"^CUST[0-9]{7}$")]
        public static partial Regex CustomerNumberRegex();
    }
}
