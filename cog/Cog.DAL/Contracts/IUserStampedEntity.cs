using System;

namespace Cog.DAL
{
    public interface IUserStampedEntity
    {
        Guid CreatedBy { get; set; }

        Guid? LastModifiedBy { get; set; }
    }
}