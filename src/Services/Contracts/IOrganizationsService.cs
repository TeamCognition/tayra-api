namespace Tayra.Services
{
    public interface IOrganizationsService
    {
        void EnsureOrganizationsAreCreatedAndMigrated();
        void Create(OrganizationCreateDTO dto);
    }
}
