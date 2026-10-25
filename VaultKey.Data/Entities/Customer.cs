using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("Customers", Schema = "bank")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CustomerNumber { get; set; } = null!;

        [Required]
        [MaxLength(1024)]
        public byte[] FullNameEnc { get; set; } = null!;

        [Required]
        [MaxLength(128)]
        public byte[] DobEnc { get; set; } = null!;

        [Required]
        [MaxLength(1024)]
        public byte[] EmailEnc { get; set; } = null!;

        [MaxLength(256)]
        public byte[]? MobileEnc { get; set; }

        [Required]
        public int KeyVersion { get; set; } = 1;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [Required]
        public bool IsActivated { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation Property: One Customer can own multiple Accounts
        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
