using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("User", Schema = "auth")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] EmailBlindIndex { get; set; } = null!;

        [Required]
        [MaxLength(1024)]
        public byte[] EmailEnc { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [Required]
        public bool TotpEnabled { get; set; } = false;

        [Required]
        public int FailedAttempts { get; set; } = 0;

        public DateTime? LockoutEnd { get; set; }

        [Required]
        public byte PasswordStrengthScore { get; set; }

        [Required]
        public DateTime PasswordExpiresAt { get; set; }

        public DateTime? PasswordChangedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        // Navigation properties
        public virtual TotpCredential? TotpCredential { get; set; }
        public virtual ICollection<ExternalLogin> ExternalLogins { get; set; } = new HashSet<ExternalLogin>();
        public virtual ICollection<RecoveryCode> RecoveryCodes { get; set; } = new HashSet<RecoveryCode>();
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new HashSet<PasswordResetToken>();
        public virtual ICollection<UserDevice> UserDevices { get; set; } = new HashSet<UserDevice>();
        public virtual ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
        public virtual ICollection<AuditLog> AuditLogs { get; set; } = new HashSet<AuditLog>();
    }
}
