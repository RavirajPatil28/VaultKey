using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("RefreshToken", Schema = "auth")]
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid FamilyId { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] TokenHash { get; set; } = null!;

        public Guid? ReplacedById { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public DateTime? UsedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(SessionId))]
        public virtual Session Session { get; set; } = null!;

        [ForeignKey(nameof(ReplacedById))]
        public virtual RefreshToken? ReplacedBy { get; set; }
    }
}
