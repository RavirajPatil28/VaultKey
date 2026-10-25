namespace VaultKey.Business.Security
{
    /// <summary>Per-column context strings, used as AES-GCM associated data and as HMAC domain separators.</summary>
    public static class EncryptionContexts
    {
        public const string CustomerFullName = "bank.Customers.FullName";
        public const string CustomerDob = "bank.Customers.Dob";
        public const string CustomerEmail = "bank.Customers.Email";
        public const string CustomerMobile = "bank.Customers.Mobile";
        public const string AccountNumber = "bank.Accounts.AccountNumber";
    }
}
