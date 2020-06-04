namespace Cog.DAL
{
    public interface IUserStampedEntity
    {
        int CreatedBy { get; set; }

        int? LastModifiedBy { get; set; }
    }
}
