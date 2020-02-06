using Firdaws.Core;
using Tayra.Common;
using Tayra.Models.Catalog;

namespace Tayra.Services
{
    public interface IIdentitiesService
    {
        void InternalCreateWithProfile(IdentityCreateDTO dto);
        Identity GetByEmail(string email);
        void InvitationJoinWithSaveChanges(IdentityJoinDTO dto);
        void CreateInvitation(int profileId, string host, IdentityInviteDTO dto);
        GridData<IdentityManageGridDTO> GetIdentityManageGridData(int profileId, IdentityManageGridParams gridParams);
        IdentityManageAssignsDTO GetIdentityManageAssignsData(int[] segmentIds, int memberProfileId);
        IdentityInvitationViewDTO GetInvitation(string InvitationCode);
        GridData<IdentityInvitationGridDTO> GetInvitationsGridData(IdentityInvitationGridParams gridParams);
        GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(int profileId, IdentityEmailsGridParams gridParams);
        void ChangePasswordWithSaveChange(int identityId, IdentityChangePasswordDTO dto);
        bool IsEmailAddressUnique(string email);
        void AddEmail(int identityId, string email);
        void SetPrimaryEmail(int identityId, string email);
        bool RemoveEmail(int identityId, string email);
        void ChangeProfileRole(ProfileRoles role, int memberProfileId, ProfileRoles toRole);
        void ArchiveProfile(ProfileRoles role, int memberProfileId);
    }
}
