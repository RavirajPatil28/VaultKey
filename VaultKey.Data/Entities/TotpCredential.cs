using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("TotpCredential", Schema = "auth")]
    public class TotpCredential
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public byte[] SecretEnc { get; set; } = null!;

        [Required]
        public long LastUsedStep { get; set; } = 0;

        public DateTime? ConfirmedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}
