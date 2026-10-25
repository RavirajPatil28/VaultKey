using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("AuditLog", Schema = "auth")]
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Guid? UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Event { get; set; } = null!;

        [StringLength(45)]
        public string? Ip { get; set; }

        [StringLength(300)]
        public string? DeviceInfo { get; set; }

        [Required]
        public DateTime At { get; set; }

        [StringLength(1000)]
        public string? Details { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
