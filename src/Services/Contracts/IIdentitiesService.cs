using Firdaws.Core;
using Tayra.Models.Catalog;

namespace Tayra.Services
{
    public interface IIdentitiesService
    {
        void InternalCreateWithProfile(IdentityCreateDTO dto);

        Identity GetByEmail(string email);

        GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(int profileId, IdentityEmailsGridParams gridParams);
        void AddEmail(int identityId, string email);
        void SetPrimaryEmail(int identityId, string email);
        bool RemoveEmail(int identityId, string email);
    }
}
