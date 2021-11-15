using System;

namespace Cog.DAL
{
    public abstract class Entity<TId>
    {
        public TId Id { get; set; } //protected set
    }

    public abstract class EntityGuidId : Entity<Guid>
    {
        protected EntityGuidId()
        {
            Id = Guid.NewGuid();
        }
    }
}