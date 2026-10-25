namespace VaultKey.Business.Security
{
    public class EncryptionOptions
    {
        public const string SectionName = "Encryption";

        /// <summary>Version used for new writes. Older versions stay in Keys so old rows can still be read.</summary>
        public int CurrentKeyVersion { get; set; } = 1;

        /// <summary>key version (as text, e.g. "1") -> base64 of a 32-byte AES-256 key.</summary>
        public Dictionary<string, string> Keys { get; set; } = new();

        /// <summary>base64 of a key (at least 32 bytes) used for HMAC blind indexes. Must differ from the AES keys.</summary>
        public string BlindIndexKey { get; set; } = string.Empty;
    }
}
