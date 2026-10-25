using System.ComponentModel.DataAnnotations;

namespace VaultKey.Business.Models
{
    public class CreateCustomerRequest : IValidatableObject
    {
        [Required, StringLength(150, MinimumLength = 2)]
        public string FullName { get; set; } = null!;

        /// <summary>JSON format: "1990-05-14".</summary>
        public DateOnly DateOfBirth { get; set; }

        [Required, EmailAddress, StringLength(256)]
        public string Email { get; set; } = null!;

        // TODO: exact 10
        /// <summary>Optional. 10 to 15 digits, optional leading +.</summary>
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Mobile must be 10 to 15 digits, with an optional leading +.")]
        public string? Mobile { get; set; }

        [Required, RegularExpression("^(Savings|Current|FixedDeposit)$", ErrorMessage = "AccountType must be Savings, Current or FixedDeposit.")]
        public string AccountType { get; set; } = null!;

        [Range(typeof(decimal), "0", "1000000000")]
        public decimal OpeningBalance { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (DateOfBirth > today.AddYears(-18))
            {
                yield return new ValidationResult(
                    "Customer must be at least 18 years old.", new[] { nameof(DateOfBirth) });
            }
            else if (DateOfBirth < today.AddYears(-120))
            {
                yield return new ValidationResult(
                    "DateOfBirth is not valid.", new[] { nameof(DateOfBirth) });
            }
        }
    }
}
