using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class ProjectsService : BaseService<OrganizationDbContext>, IProjectsService
    {
        #region Constructor

        public ProjectsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public IQueryable<Project> Get()
        {
            return DbContext.Projects
                .AsNoTracking();
        }

        public Project Get(int id)
        {
            return Get()
                .FirstOrDefault(i => i.Id == id);
        }

        public Project Get(string projectKey)
        {
            return Get()
                .FirstOrDefault(i => i.Key == projectKey);
        }

        public Project Create(ProjectCreateDTO dto)
        {
            var project = new Project
            {
                Key = dto.Key,
                Name = dto.Name,
                Timezone = dto.Timezone,
                OrganizationId = 1
            };

            DbContext.Projects.Add(project);
            DbContext.SaveChanges();
            return project;
        }

        public bool Delete(int id)
        {
            var projectToDelete = GetOrFail(id);
            DbContext.Projects.Remove(projectToDelete);
            var affectedRecords = DbContext.SaveChanges();
            return affectedRecords > 0;
        }

        #endregion

        private Project GetOrFail(int id)
        {
            var item = Get(id);
            if (item == null)
            {
                throw new ApplicationException($"{typeof(Project)} does not exist.");
            }

            return item;
        }
    }
}
