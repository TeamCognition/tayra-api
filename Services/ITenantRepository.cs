namespace Tayra.Services
{
    public interface ITenantRepository
    {
        OrganizationModel GetVenueDetails(int tenantId);
    }
}
