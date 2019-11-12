using Firdaws.Core;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IIdentitiesService
    {
        void Create(IdentityCreateDTO dto);

        Identity GetById(int identityId);
        Identity GetByUsername(string username);
        Identity GetByEmail(string email);

        GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(int profileId, IdentityEmailsGridParams gridParams);
        void AddEmail(int identityId, string email);
        void SetPrimaryEmail(int identityId, string email);
        bool RemoveEmail(int identityId, string email);
    }
}
