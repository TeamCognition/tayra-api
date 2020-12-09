using System;
using Cog.Core;
using Tayra.Common;
using Tayra.Models.Catalog;

namespace Tayra.Services
{
    public interface IIdentitiesService
    {
        void InternalCreateWithProfile(IdentityCreateDTO dto);
        Identity GetByEmail(string email);
        void InvitationJoinWithSaveChanges(IdentityJoinDTO dto);
        void CreateInvitation(string host, IdentityInviteDTO dto);
        void ResendInvitation(string host, Guid invitationId);
        GridData<IdentityInvitationGridDTO> GetInvitationsGridData(IdentityInvitationGridParams gridParams);
        IdentityInvitationViewDTO GetInvitation(string invitationCode);
        void DeleteInvitation(Guid invitationId);
        GridData<IdentityManageGridDTO> GetIdentityManageGridData(Guid profileId, ProfileRoles role, IdentityManageGridParams gridParams);
        IdentityManageAssignsDTO GetIdentityManageAssignsData(Guid[] segmentIds, Guid memberProfileId);
        GridData<IdentityEmailsGridDTO> GetIdentityEmailsGridData(Guid profileId, IdentityEmailsGridParams gridParams);
        void ChangePasswordWithSaveChange(Guid identityId, IdentityChangePasswordDTO dto);
        bool IsEmailAddressUnique(string email);
        void AddEmail(Guid identityId, Guid profileId, string email);
        void SetPrimaryEmail(Guid identityId, Guid profileId, string email);
        bool RemoveEmail(Guid identityId, string email);
        void ChangeProfileRole(ProfileRoles role, Guid memberProfileId, ProfileRoles toRole);
        void ArchiveProfile(ProfileRoles role, Guid memberProfileId);
    }
}
