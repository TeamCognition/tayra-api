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
        GridData<IdentityInvitationGridDTO> GetInvitationsGridData(IdentityInvitationGridParams gridParams);
        IdentityInvitationViewDTO GetInvitation(string InvitationCode);
        void DeleteInvitation(int invitationId);
        GridData<IdentityManageGridDTO> GetIdentityManageGridData(int profileId, ProfileRoles role, IdentityManageGridParams gridParams);
        IdentityManageAssignsDTO GetIdentityManageAssignsData(int[] segmentIds, int memberProfileId);
        GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(int profileId, IdentityEmailsGridParams gridParams);
        void ChangePasswordWithSaveChange(int identityId, IdentityChangePasswordDTO dto);
        bool IsEmailAddressUnique(string email);
        void AddEmail(int identityId, int profileId, string email);
        void SetPrimaryEmail(int identityId, int profileId, string email);
        bool RemoveEmail(int identityId, string email);
        void ChangeProfileRole(ProfileRoles role, int memberProfileId, ProfileRoles toRole);
        void ArchiveProfile(ProfileRoles role, int memberProfileId);
    }
}
