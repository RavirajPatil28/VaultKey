using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("UserDevice", Schema = "auth")]
    public class UserDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] DeviceTokenHash { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string DeviceName { get; set; } = null!;

        [Required]
        public DateTime FirstSeenAt { get; set; }

        [Required]
        public DateTime LastSeenAt { get; set; }

        [StringLength(45)]
        public string? LastIp { get; set; }

        public DateTime? NotifiedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Session> Sessions { get; set; } = new HashSet<Session>();
    }
}
