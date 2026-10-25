namespace VaultKey.Business.Security.Interfaces
{
    public interface IFieldEncryptor
    {
        /// <summary>Key version that Encrypt will use for new values.</summary>
        int CurrentKeyVersion { get; }

        /// <summary>
        /// AES-256-GCM encrypt. <paramref name="context"/> (for example "bank.Customers.Email") is bound to the
        /// ciphertext as associated data, so a value copied into a different column fails to decrypt.
        /// Output layout: [1 byte key version][12 byte nonce][16 byte tag][ciphertext].
        /// </summary>
        byte[] Encrypt(string plaintext, string context);

        string Decrypt(byte[] payload, string context);

        /// <summary>HMAC-SHA256 (32 bytes) of an already-normalized value, for exact-match lookup and uniqueness.</summary>
        byte[] ComputeBlindIndex(string normalizedValue, string context);
    }
}
