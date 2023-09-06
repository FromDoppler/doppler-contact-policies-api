using Doppler.ContactPolicies.Data.Access.Entities;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings
{
    public interface IContactPoliciesSettingsRepository
    {
        Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsByIdUserAsync(int idUser);
        Task UpdateContactPoliciesSettingsAsync(int idUser, Entities.ContactPoliciesSettings contactPoliciesToInsert, ContactPoliciesTimeRestriction contactPoliciesTimeRestrictionToInsert);
        Task<int?> GetIdUserByAccountName(string accountName);
    }
}
