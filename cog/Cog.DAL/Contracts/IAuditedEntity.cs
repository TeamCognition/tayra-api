﻿namespace Cog.DAL
{
    public interface IAuditedEntity : /*IDTO,*/ ITimeStampedEntity, IUserStampedEntity
    {
    }
}