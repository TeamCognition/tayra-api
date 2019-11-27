namespace Tayra.Models.Organizations
{
    public class TenantDTO
    {
        public string Host { get; set; }
        public int ShardingKey { get; set; }
    }
}
