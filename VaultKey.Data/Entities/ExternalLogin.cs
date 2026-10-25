using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaultKey.Data.Entities
{
    [Table("ExternalLogin", Schema = "auth")]
    public class ExternalLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(30)]
        public string Provider { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string ProviderSubjectId { get; set; } = null!;

        [Required]
        public DateTime LinkedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}
