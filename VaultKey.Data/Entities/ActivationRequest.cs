using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("ActivationRequest", Schema = "auth")]
    public class ActivationRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] OtpHash { get; set; } = null!;

        [Required]
        public int Attempts { get; set; } = 0;

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [StringLength(45)]
        public string? Ip { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
