using System;
using System.Collections.Generic;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Analytics;
using Tayra.Analytics.Metrics;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    [ApiController]
    public class SegmentsController : BaseController
    {
        #region Constructor

        public SegmentsController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        protected OrganizationDbContext DbContext { get; set; }

        #endregion

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<SegmentGridDTO>> Search([FromBody] SegmentGridParams gridParams)
        {
            return SegmentsService.GetGridData(CurrentUser.SegmentsIds, gridParams);
        }

        [HttpPost("{segmentKey}/searchMembers")]
        public ActionResult<GridData<SegmentMemberGridDTO>> SearchMembers([FromRoute] string segmentKey, [FromBody] SegmentMemberGridParams gridParams)
        {
            return SegmentsService.GetSegmentMembersGridData(segmentKey, gridParams);
        }

        [HttpPost("{segmentKey}/searchTeams")]
        public ActionResult<GridData<SegmentTeamGridDTO>> SearchTeams([FromRoute] string segmentKey, [FromBody] SegmentTeamGridParams gridParams)
        {
            return SegmentsService.GetSegmentTeamsGridData(segmentKey, gridParams);
        }

        [HttpGet]
        public ActionResult<SegmentViewDTO> GetSegment([FromQuery] string segmentKey)
        {
            return SegmentsService.GetSegmnetViewDTO(segmentKey);
        }

        [HttpGet("{segmentKey}/averageMetrics")]
        public ActionResult<Dictionary<int,AnalyticsMetricWithIterationSplitDto>> GetSegmentAverageMetrics([FromRoute] string segmentKey)
        {
            return SegmentsService.GetSegmentAverageMetrics(segmentKey);
        }

        [HttpGet("{segmentKey}/rawScore")]
        public ActionResult<SegmentRawScoreDTO> GetSegmentRowScore([FromRoute] string segmentKey)
        {
            return SegmentsService.GetSegmentRawScore(segmentKey);
        }

        [HttpGet("{segmentKey}/rankChart")]
        public Dictionary<int, MetricsValueWEntity[]>  GetSegmentRankChart([FromRoute] string segmentKey)
        {
            return SegmentsService.GetSegmentRankChart(segmentKey);
        }

        [HttpPost]
        public IActionResult Create([FromBody] SegmentCreateDTO dto)
        {
            SegmentsService.Create(CurrentUser.ProfileId, CurrentUser.Role, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromQuery] int segmentId, [FromBody] SegmentCreateDTO dto)
        {
            SegmentsService.Update(segmentId, dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Archive([FromQuery] int segmentId)
        {
            SegmentsService.Archive(segmentId);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpPost("addMember")]
        public IActionResult AddMember([FromBody] SegmentMemberAddRemoveDTO[] dtos)
        {
            foreach (var dto in dtos)
            {
                SegmentsService.AddMember(dto);
            }

            DbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("removeMember")]
        public IActionResult RemoveMember([FromQuery] SegmentMemberAddRemoveDTO dto)
        {
            SegmentsService.RemoveMember(dto);
            DbContext.SaveChanges();
            return Ok();
        }

        [HttpGet, Route("{segmentKey}/integrations")]
        public ActionResult<List<IntegrationSegmentViewDTO>> GetSegmentIntegrations([FromRoute] string segmentKey)
        {
            return IntegrationsService.GetSegmentIntegrations(segmentKey);
        }
        
        [HttpGet("rawMetrics")]
        public TableData GetRawMetrics([FromQuery] int m, [FromQuery] int entityId, [FromQuery] string period)
        {
            var datePeriod = new DatePeriod(period);
            var metricType = MetricType.FromValue(m) as PureMetric;
            
            return new TableData(metricType.TypeOfRawMetric, metricType.GetRawMetrics(DbContext, datePeriod, entityId, EntityTypes.Segment));
        }
        
        [HttpGet("statsWidget/{segmentId:int}")]
        public ActionResult<SegmentStatsDTO> GetSegmentStats(int segmentId)
        {
            return SegmentsService.GetSegmentStats(segmentId);
        }

        #endregion
    }
}