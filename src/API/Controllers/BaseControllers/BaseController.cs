using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tayra.Common;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class BaseController : ControllerBase
    {
        #region Private Members

        private ILogsService _logsService;
        private IBlobsService _blobsService;
        private IItemsService _itemsService;
        private IShopsService _shopsService;
        private ITasksService _tasksService;
        private ITeamsService _teamsService;
        private IQuestsService _questsService;
        private IPraiseService _praiseService;
        private ITokensService _tokensService;
        private ILookupsService _lookupsService;
        private IReportsService _reportsService;
        private IProfilesService _profilesService;
        private ISegmentsService _segmentsService;
        private IAnalyticsService _analyticsService;
        private IShopItemsService _shopItemsService;
        private IAssistantService _assistantService;
        private IIdentitiesService _identitiesService;
        private IInventoriesService _inventoriesService;
        private IClaimBundlesService _claimBundlesService;
        private IIntegrationsService _integrationsService;

        private TayraPrincipal _currentUser;

        #region OvoOno

        public readonly IConfiguration Configuration;

        #endregion
        #endregion

        #region Constructor

        public BaseController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Configuration = Resolve<IConfiguration>();
        }

        #endregion

        #region Services Properties

        protected IServiceProvider ServiceProvider { get; }
        protected ILogsService LogsService => _logsService ?? (_logsService = Resolve<ILogsService>());
        protected IBlobsService BlobsService => _blobsService ?? (_blobsService = Resolve<IBlobsService>());
        protected IItemsService ItemsService => _itemsService ?? (_itemsService = Resolve<IItemsService>());
        protected IShopsService ShopsService => _shopsService ?? (_shopsService = Resolve<IShopsService>());
        protected ITasksService TasksService => _tasksService ?? (_tasksService = Resolve<ITasksService>());
        protected ITeamsService TeamsService => _teamsService ?? (_teamsService = Resolve<ITeamsService>());
        protected IPraiseService PraiseService => _praiseService ?? (_praiseService = Resolve<IPraiseService>());
        protected ITokensService TokensService => _tokensService ?? (_tokensService = Resolve<ITokensService>());
        protected IQuestsService QuestsService => _questsService ?? (_questsService = Resolve<IQuestsService>());
        protected ILookupsService LookupsService => _lookupsService ?? (_lookupsService = Resolve<ILookupsService>());
        protected IReportsService ReportsService => _reportsService ?? (_reportsService = Resolve<IReportsService>());
        protected IProfilesService ProfilesService => _profilesService ?? (_profilesService = Resolve<IProfilesService>());
        protected ISegmentsService SegmentsService => _segmentsService ?? (_segmentsService = Resolve<ISegmentsService>());
        protected IAnalyticsService AnalyticsService => _analyticsService ?? (_analyticsService = Resolve<IAnalyticsService>());
        protected IAssistantService AssistantService => _assistantService ?? (_assistantService = Resolve<IAssistantService>());
        protected IShopItemsService ShopItemsService => _shopItemsService ?? (_shopItemsService = Resolve<IShopItemsService>());
        protected IIdentitiesService IdentitiesService => _identitiesService ?? (_identitiesService = Resolve<IIdentitiesService>());
        protected IInventoriesService InventoriesService => _inventoriesService ?? (_inventoriesService = Resolve<IInventoriesService>());
        protected IClaimBundlesService ClaimBundlesService => _claimBundlesService ?? (_claimBundlesService = Resolve<IClaimBundlesService>());
        protected IIntegrationsService IntegrationsService => _integrationsService ?? (_integrationsService = Resolve<IIntegrationsService>());

        #endregion

        #region Other Properties

        /// <summary>
        /// Returns current TayraPrincipal.
        /// </summary>
        public TayraPrincipal CurrentUser => _currentUser ?? (_currentUser = new TayraPrincipal(User));

        #endregion

        #region Helper Methods

        protected T Resolve<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        #endregion
    }
}