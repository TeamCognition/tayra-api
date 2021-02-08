using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Mailer.Contracts;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class LogsService : BaseService<OrganizationDbContext>, ILogsService
    {
        #region Constructor

        public LogsService(OrganizationDbContext dbContext,IMailerService mailerService) : base(dbContext)
        {
            MailerService = mailerService;
        }

        #endregion

        private IMailerService MailerService { get; }
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

        public void SendLog(Guid profileId, LogEvents logEvent, IEmailTemplate template)
        {
            var devices = DbContext.LogDevices.Where(x => x.ProfileId == profileId).Select(x => new
            {
                LogDeviceType = x.Type,
                Address = x.Address,
                IsEnabled = x.Settings.Any(s => s.LogEvent == logEvent && s.IsEnabled == false)
            }).ToList();

            foreach (var d in devices)
            {
                MailerService.SendEmail(d.Address,EmailService.noReplyAddress,template);
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
                query = from pl in query
                    where gridParams.ProfileIds.Contains(pl.ProfileId)
                    select pl;
 
            }
            if (gridParams.TeamIds.Length > 0)
            {
                var tm = DbContext.ProfileAssignments.Where(x => gridParams.TeamIds.Contains(x.TeamId.Value)).Select(x => x.ProfileId).ToArray();
 
                query = from pl in query
                    where tm.Contains(pl.ProfileId)
                    select pl;
            }
            if (gridParams.SegmentIds.Length > 0)
            {
                var sm = DbContext.ProfileAssignments.Where(x => gridParams.SegmentIds.Contains(x.SegmentId)).Select(x => x.ProfileId).ToArray();
 
                query = from pl in query
                    where sm.Contains(pl.ProfileId)
                    select pl;
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

            
            GridData<LogGridDTO> gridData = query.Select(pl=> new LogGridDTO
            {
                Data = JsonConvert.DeserializeObject(pl.Log.Data),
                Event = pl.Event,
                Created = pl.Log.Created,
                Message = pl.Log.Description,
                Uri = pl.Log.ExternalUrl,
                ProfileAvatar = pl.Profile.Avatar,
                ProfileFullName = pl.Profile.FirstName + " " + pl.Profile.LastName,
                ProfileUsername = pl.Profile.Username,
                IsAssistedByTayra = pl.Log.IsAssistedByTayra
            }).GetGridData(gridParams);

            return gridData;
        }

        #endregion
    }
}
