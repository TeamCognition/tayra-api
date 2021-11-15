using System;

namespace Cog.DAL
{
    public interface ITimeStampedEntity
    {
        DateTime Created { get; set; }

        DateTime? LastModified { get; set; }
    }
}