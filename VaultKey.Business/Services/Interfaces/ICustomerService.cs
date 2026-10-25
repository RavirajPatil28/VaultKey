using VaultKey.Business.Models;
using static VaultKey.Business.Models.CustomerResponses;

namespace VaultKey.Business.Services.Interfaces
{
    public interface ICustomerService
    {
        /// <summary>Creates a customer and one account in a single transaction. All personal data is stored encrypted.</summary>
        Task<CustomerCreatedResponse> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default);

        /// <summary>Returns profile details only (no accounts), or null when the customer number is unknown.</summary>
        Task<CustomerProfileResponse?> GetProfileAsync(string customerNumber, CancellationToken ct = default);

    }
}
