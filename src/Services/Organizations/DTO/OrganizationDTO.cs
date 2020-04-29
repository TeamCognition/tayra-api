namespace Tayra.Services
{
    public class OrganizationDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
        //model props
        public string DatabaseName { get; set; }

        public string DatabaseServerName { get; set; }
    }
}
