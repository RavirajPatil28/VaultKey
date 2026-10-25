using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("Accounts", Schema = "bank")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [MaxLength(256)]
        public byte[] AccountNumberEnc { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public byte[] AccountNumberBlindIndex { get; set; } = null!;

        [Required]
        [StringLength(4), MinLength(4)]
        public string AccountLast4 { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string AccountType { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        [StringLength(3), MinLength(3)]
        public string Currency { get; set; } = "INR";

        [Required]
        public int KeyVersion { get; set; } = 1;

        // Navigation Property: The parent Customer configuration
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; } = null!;
    }
}
