using System.Linq;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IProjectsService
    {
        IQueryable<Project> Get();
        Project Get(int id);
        Project Get(string projectKey);
        Project Create(ProjectCreateDTO dto);
        bool Delete(int id);
    }
}