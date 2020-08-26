using Tayra.Models.Organizations;

namespace Tayra.Services.Repositories
{
    public class RepositoriesService : BaseService<OrganizationDbContext>, IRepositoriesService
    {
        #region Constructor

        public RepositoriesService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        #endregion
        
    }
}