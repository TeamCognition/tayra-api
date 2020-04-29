namespace Tayra.Models.Organizations
{
    public static class OrganizationsService
    {
        public static void InsertOrganization(string connectionString, Organization organization)
        {
            using (var db = new OrganizationDbContext(connectionString))
            {
                db.Organizations.Add(organization);
                db.SaveChanges();
            }
        }
    }
}
