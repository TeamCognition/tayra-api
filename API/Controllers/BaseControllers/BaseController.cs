﻿using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class BaseController : ControllerBase
    {
        #region Private Members

        private ILogsService _logsService;
        private IShopsService _shopsService;
        private ITasksService _tasksService;
        private ITeamsService _teamsService;
        private ITokensService _tokensService;
        private ILookupsService _lookupsService;
        private IReportsService _reportsService;
        private IProfilesService _profilesService;
        private IProjectsService _projectsService;
        private IShopItemsService _shopItemsService;
        private IChallengesService _challengesService;
        private IIdentitiesService _identitiesService;
        private IInventoriesService _inventoriesService;
        private IClaimBundlesService _claimBundlesService;
        private ICompetitionsService _competitionsService;
        private IIntegrationsService _integrationsService;

        private Profile _currentUser;

        #endregion

        #region Constructor

        public BaseController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _currentUser = LoadCurrentUser();
        }

        #endregion

        #region Services Properties

        protected IServiceProvider ServiceProvider { get; }
        protected ILogsService LogsService => _logsService ?? (_logsService = Resolve<ILogsService>());
        protected IShopsService ShopsService => _shopsService ?? (_shopsService = Resolve<IShopsService>());
        protected ITasksService TasksService => _tasksService ?? (_tasksService = Resolve<ITasksService>());
        protected ITeamsService TeamsService => _teamsService ?? (_teamsService = Resolve<ITeamsService>());
        protected ITokensService TokensService => _tokensService ?? (_tokensService = Resolve<ITokensService>());
        protected ILookupsService LookupsService => _lookupsService ?? (_lookupsService = Resolve<ILookupsService>());
        protected IReportsService ReportsService => _reportsService ?? (_reportsService = Resolve<IReportsService>());
        protected IProfilesService ProfilesService => _profilesService ?? (_profilesService = Resolve<IProfilesService>());
        protected IProjectsService ProjectsService => _projectsService ?? (_projectsService = Resolve<IProjectsService>());
        protected IShopItemsService ShopItemsService => _shopItemsService ?? (_shopItemsService = Resolve<IShopItemsService>());
        protected IChallengesService ChallengesService => _challengesService ?? (_challengesService = Resolve<IChallengesService>());
        protected IIdentitiesService IdentitiesService => _identitiesService ?? (_identitiesService = Resolve<IIdentitiesService>());
        protected IInventoriesService InventoriesService => _inventoriesService ?? (_inventoriesService = Resolve<IInventoriesService>());
        protected IClaimBundlesService ClaimBundlesService => _claimBundlesService ?? (_claimBundlesService = Resolve<IClaimBundlesService>());
        protected ICompetitionsService CompetitionsService => _competitionsService ?? (_competitionsService = Resolve<ICompetitionsService>());
        protected IIntegrationsService IntegrationsService => _integrationsService ?? (_integrationsService = Resolve<IIntegrationsService>());

        #endregion

        #region Other Properties

        /// <summary>
        /// Returns current TayraPrincipal.
        /// </summary>
        //protected new FirdawsPrincipal User => new FirdawsPrincipal(HttpContext.User);


        public Profile CurrentUser => _currentUser ?? (_currentUser = LoadCurrentUser());

        #endregion

        #region Helper Methods

        protected T Resolve<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        protected Profile LoadCurrentUser()
        {
            if (User?.Identity is ClaimsIdentity identity)
            {
                var username = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return ProfilesService.GetByUsername(username);
            }

            return null;
        }

        #endregion
    }
}