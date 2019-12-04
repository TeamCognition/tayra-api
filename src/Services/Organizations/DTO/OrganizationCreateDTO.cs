namespace Tayra.Services
{
    public class OrganizationCreateDTO
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Timezone { get; set; }

        public string DatabaseName { get; set; }
        public string DatabaseServer { get; set; }

        public string TemplateConnectionString { get; set; }
    }
}
