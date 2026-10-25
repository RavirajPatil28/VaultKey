using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("Session", Schema = "auth")]
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [StringLength(300)]
        public string? DeviceInfo { get; set; }

        [StringLength(45)]
        public string? Ip { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime LastSeenAt { get; set; }

        [Required]
        public DateTime AbsoluteExpiry { get; set; }

        public DateTime? RevokedAt { get; set; }

        [StringLength(30)]
        public string? RevokeReason { get; set; }

        public Guid? DeviceId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(DeviceId))]
        public virtual UserDevice? Device { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
