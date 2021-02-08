﻿using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class LogsService : BaseService<OrganizationDbContext>, ILogsService
    {
        #region Constructor

        public LogsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public void LogEvent(LogCreateDTO dto)
        {
            var log = new Log
            {
                Event = dto.Event,
                Data = JsonConvert.SerializeObject(dto.Data)
            };

            DbContext.Logs.Add(log);

            if (dto.ProfileId.HasValue)
            {
                DbContext.ProfileLogs.Add(new ProfileLog
                {
                    Event = dto.Event,
                    Log = log,
                    ProfileId = dto.ProfileId.Value
                });
            }

            if (dto.ShopId.HasValue)
            {
                DbContext.ShopLogs.Add(new ShopLog
                {
                    Event = dto.Event,
                    Log = log,
                    ShopId = dto.ShopId.Value,
                });
            }

        }

        public void SendLog(Guid profileId, LogEvents logEvent, ITemplateEmailDTO dto)
        {
            var devices = DbContext.LogDevices.Where(x => x.ProfileId == profileId).Select(x => new
            {
                LogDeviceType = x.Type,
                Address = x.Address,
                IsEnabled = x.Settings.Where(s => s.LogEvent == logEvent && s.IsEnabled == false).Any()
            }).ToList();

            foreach (var d in devices)
            {
                MailerService.SendEmail(d.Address, dto);
            }
        }

        public GridData<LogGridDTO> GetGridData(LogGridParams gridParams)
        {
            IQueryable<ProfileLog> query = DbContext.ProfileLogs;
            
            if (gridParams.ProfileIds.Length == 0)
            {
                query = query.Where(x => x.Event != LogEvents.ProfilePraiseReceived
                                         && x.Event != LogEvents.InventoryItemGiftReceived);
            }
            else
            {
                query = from l in query
                    join pl in DbContext.ProfileLogs on l.Log.Id equals pl.LogId
                    where gridParams.ProfileIds.Contains(pl.ProfileId)
                    select l;
 
            }
            if (gridParams.TeamIds.Length > 0)
            {
                var tm = DbContext.ProfileAssignments.Where(x => gridParams.TeamIds.Contains(x.TeamId.Value)).Select(x => x.ProfileId).ToArray();
 
                query = from l in query
                    join pl in DbContext.ProfileLogs on l.Log.Id equals pl.LogId
                    where tm.Contains(pl.ProfileId)
                    select l;
            }
            if (gridParams.SegmentIds.Length > 0)
            {
                var sm = DbContext.ProfileAssignments.Where(x => gridParams.SegmentIds.Contains(x.SegmentId)).Select(x => x.ProfileId).ToArray();
 
                query = from l in query
                    join pl in DbContext.ProfileLogs on l.Log.Id equals pl.LogId
                    where sm.Contains(pl.ProfileId)
                    select l;
            }
            // if (gridParams.ShopLogs.HasValue && gridParams.ShopLogs.Value)
            // {
            //     var shopId = DbContext.Shops.Select(x => x.Id).FirstOrDefault();
            //
            //     query = from l in query
            //             join sl in DbContext.ShopLogs on l.Log.Id equals sl.LogId
            //             where sl.ShopId == shopId
            //             select sl;
            // }

            IQueryable<Log> logsQuery= from l in DbContext.Logs
                join pl in query on l.Id equals pl.LogId
                select l; 
            GridData<LogGridDTO> gridData= logsQuery.Select(l=> new LogGridDTO
            {
                Data = JsonConvert.DeserializeObject(l.Data),
                Event = l.Event,
                Created = l.Created,
                Description = l.Description,
                DescriptionLink = l.DescriptionLink,
                AuthorAvatar = l.AuthorAvatar,
                AuthorName = l.AuthorName,
                AuthorUsername = l.AuthorUsername,
                IsGuidedByTayra = l.IsGuidedByTayra
            }).GetGridData(gridParams);

            return gridData;
        }

        #endregion
    }
}
