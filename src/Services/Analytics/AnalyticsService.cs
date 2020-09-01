using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services.Analytics
{
    public class AnalyticsService : BaseService<OrganizationDbContext>, IAnalyticsService
    {
        
        #region Constructor

        public AnalyticsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }
        
        #endregion
        
        #region Public Methods

        public AnalyticsFilterRowDTO GetAnalyticsFilterRows(FilterRowBodyDTO body)
        {
            
            List<AnalyticsFilterRowDTO.AnalyticsEntity> rows = new List<AnalyticsFilterRowDTO.AnalyticsEntity>();;
            
            foreach(var x in body.Rows)
            {
                if (x.Type == "profile")
                {
                    rows.Add((from prw in DbContext.ProfileReportsWeekly
                        where prw.ProfileId == x.Id
                        group prw by prw.DateId into g
                        select new AnalyticsFilterRowDTO.AnalyticsEntity
                        {
                            Id = g.Select(y => y.ProfileId).FirstOrDefault(),
                            Name = g.Select(y => y.Profile.FirstName + " " + y.Profile.LastName).FirstOrDefault(),
                            Type = x.Type,
                            MetricsValues = (new AnalyticsFilterRowDTO.AnalyticsEntity.Metric[]
                            {
                                new AnalyticsFilterRowDTO.AnalyticsEntity.Metric
                                {
                                    Id = MetricTypes.Assist,
                                    Averages = g.Select(m => m.OImpactAverage).FirstOrDefault()
                                },
                                new AnalyticsFilterRowDTO.AnalyticsEntity.Metric
                                {
                                    Id = MetricTypes.Assist,
                                    Averages = g.Select(m => m.OImpactAverage).FirstOrDefault()
                                }
                            })
                        }).FirstOrDefault());
                } else if (x.Type == "segment")
                {
                    rows.Add((from srw in DbContext.SegmentReportsWeekly
                        where srw.SegmentId == x.Id
                        group srw by srw.DateId into g
                        select new AnalyticsFilterRowDTO.AnalyticsEntity
                        {
                            Id = g.Select(y => y.SegmentId).FirstOrDefault(),
                            Name = g.Select(y => y.Segment.Name).FirstOrDefault(),
                            Type = x.Type,
                            MetricsValues = (new AnalyticsFilterRowDTO.AnalyticsEntity.Metric[]
                            {
                                new AnalyticsFilterRowDTO.AnalyticsEntity.Metric
                                {
                                    Id = MetricTypes.Assist,
                                    Averages = g.Select(m => m.OImpactAverage).FirstOrDefault()
                                },
                                new AnalyticsFilterRowDTO.AnalyticsEntity.Metric
                                {
                                    Id = MetricTypes.Assist,
                                    Averages = g.Select(m => m.OImpactAverage).FirstOrDefault()
                                }
                            })
                        }).FirstOrDefault());
                } else if (x.Type == "team")
                {
                    rows.Add((from trw in DbContext.TeamReportsWeekly
                        where trw.SegmentId == x.Id
                        group trw by trw.DateId into g
                        select new AnalyticsFilterRowDTO.AnalyticsEntity
                        {
                            Id = g.Select(y => y.SegmentId).FirstOrDefault(),
                            Name = g.Select(y => y.Team.Name).FirstOrDefault(),
                            Type = x.Type,
                            MetricsValues = (new AnalyticsFilterRowDTO.AnalyticsEntity.Metric[]
                            {
                                new AnalyticsFilterRowDTO.AnalyticsEntity.Metric
                                {
                                    Id = MetricTypes.Assist,
                                    Averages = g.Select(m => m.OImpactAverage).FirstOrDefault()
                                },
                                new AnalyticsFilterRowDTO.AnalyticsEntity.Metric
                                {
                                    Id = MetricTypes.Assist,
                                    Averages = g.Select(m => m.OImpactAverage).FirstOrDefault()
                                }
                            })
                        }).FirstOrDefault());
                }
                
            }

            return new AnalyticsFilterRowDTO
            {
                MetricsRows = rows
            };
        }
        
        #endregion
    }
}