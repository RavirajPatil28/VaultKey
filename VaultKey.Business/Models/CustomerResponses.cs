namespace VaultKey.Business.Models
{
    public class CustomerResponses
    {
        public record CustomerCreatedResponse(
            string CustomerNumber,
            string FullName,
            string AccountNumberMasked,
            string AccountType,
            decimal Balance,
            string Currency,
            DateTime CreatedAt);

        public record CustomerProfileResponse(
            string CustomerNumber,
            string FullName,
            DateOnly DateOfBirth,
            string Email,
            string? Mobile,
            string Status,
            bool IsActivated,
            DateTime CreatedAt);
    }
}
