using System;
using Microsoft.EntityFrameworkCore;

namespace Cog.DAL
{
    public interface IAuditPersistenceStore : IDisposable
    {
        DbSet<EntityChangeLog> EntityChangeLogs { get; set; }
    }
}