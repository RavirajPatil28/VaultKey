using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using VaultKey.Business.Security.Interfaces;

namespace VaultKey.Business.Security
{
    public sealed class AesGcmFieldEncryptor : IFieldEncryptor
    {
        private const int NonceSize = 12;
        private const int TagSize = 16;
        private const int HeaderSize = 1 + NonceSize + TagSize;

        private readonly Dictionary<int, byte[]> _keys;
        private readonly byte[] _blindIndexKey;

        public int CurrentKeyVersion { get; }

        public AesGcmFieldEncryptor(IOptions<EncryptionOptions> options)
        {
            var o = options.Value;

            if (o.Keys is null || o.Keys.Count == 0)
            {
                throw new InvalidOperationException(
                    "No encryption keys configured. Set Encryption:Keys:1 (base64, 32 bytes) via user-secrets or Key Vault.");
            }

            _keys = new Dictionary<int, byte[]>();
            foreach (var (version, base64) in o.Keys)
            {
                if (!int.TryParse(version, out var v) || v is < 1 or > 255)
                {
                    throw new InvalidOperationException($"Encryption key version '{version}' must be a number from 1 to 255.");
                }

                var key = Convert.FromBase64String(base64);
                if (key.Length != 32)
                {
                    throw new InvalidOperationException($"Encryption key {v} must be exactly 32 bytes (base64 encoded).");
                }

                _keys[v] = key;
            }

            if (!_keys.ContainsKey(o.CurrentKeyVersion))
            {
                throw new InvalidOperationException($"CurrentKeyVersion {o.CurrentKeyVersion} has no matching key.");
            }

            CurrentKeyVersion = o.CurrentKeyVersion;

            if (string.IsNullOrWhiteSpace(o.BlindIndexKey))
            {
                throw new InvalidOperationException("Encryption:BlindIndexKey is not configured.");
            }

            _blindIndexKey = Convert.FromBase64String(o.BlindIndexKey);
            if (_blindIndexKey.Length < 32)
            {
                throw new InvalidOperationException("Encryption:BlindIndexKey must be at least 32 bytes (base64 encoded).");
            }
        }

        public byte[] Encrypt(string plaintext, string context)
        {
            ArgumentNullException.ThrowIfNull(plaintext);
            ArgumentException.ThrowIfNullOrEmpty(context);

            var plainBytes = Encoding.UTF8.GetBytes(plaintext);
            var payload = new byte[HeaderSize + plainBytes.Length];

            payload[0] = (byte)CurrentKeyVersion;
            var nonce = payload.AsSpan(1, NonceSize);
            var tag = payload.AsSpan(1 + NonceSize, TagSize);
            var cipher = payload.AsSpan(HeaderSize);

            RandomNumberGenerator.Fill(nonce);

            using var aes = new AesGcm(_keys[CurrentKeyVersion], TagSize);
            aes.Encrypt(nonce, plainBytes, cipher, tag, Encoding.UTF8.GetBytes(context));

            return payload;
        }

        public string Decrypt(byte[] payload, string context)
        {
            ArgumentNullException.ThrowIfNull(payload);
            ArgumentException.ThrowIfNullOrEmpty(context);

            if (payload.Length < HeaderSize)
            {
                throw new CryptographicException("Encrypted value is too short.");
            }

            var version = payload[0];
            if (!_keys.TryGetValue(version, out var key))
            {
                throw new CryptographicException($"No key configured for version {version}.");
            }

            var nonce = payload.AsSpan(1, NonceSize);
            var tag = payload.AsSpan(1 + NonceSize, TagSize);
            var cipher = payload.AsSpan(HeaderSize);
            var plain = new byte[cipher.Length];

            using var aes = new AesGcm(key, TagSize);
            aes.Decrypt(nonce, cipher, tag, plain, Encoding.UTF8.GetBytes(context));

            return Encoding.UTF8.GetString(plain);
        }

        public byte[] ComputeBlindIndex(string normalizedValue, string context)
        {
            ArgumentNullException.ThrowIfNull(normalizedValue);
            ArgumentException.ThrowIfNullOrEmpty(context);

            return HMACSHA256.HashData(_blindIndexKey, Encoding.UTF8.GetBytes(context + "|" + normalizedValue));
        }
    }

}
