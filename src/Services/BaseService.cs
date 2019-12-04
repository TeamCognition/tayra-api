using Microsoft.EntityFrameworkCore;

namespace Tayra.Services
{
    public abstract class BaseService<TContext> where TContext : DbContext
    {
        #region Constructor

        protected BaseService(TContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected TContext DbContext { get; }

        #endregion

        #region Public Methods

        #endregion
    }
}