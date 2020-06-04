using System;
using System.Linq;
using Cog.Core;
using Cog.DAL;
using Newtonsoft.Json;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class DemoLogsService : BaseService<OrganizationDbContext>, ILogsService
    {
        #region Constructor

        public DemoLogsService(OrganizationDbContext dbContext) : base(dbContext)
        {
        }

        #endregion

        #region Public Methods

        public void LogEvent(LogCreateDTO dto)
        {
            Random rnd = new Random();
            var created = DateTime.UtcNow.AddDays(rnd.Next(-30, -1)).AddMinutes(rnd.Next(-150, -1));

            if (dto.Data.TryGetValue("timestamp", out string value))
            {
                created = DateTime.Parse(value);

                //if its utcNow, randomize it
                if (created >= DateTime.UtcNow.AddMinutes(-5))
                {
                    dto.Data["timestamp"] = created.ToString();
                }
            }
            var log = new Log
            {
                Event = dto.Event,
                Data = JsonConvert.SerializeObject(dto.Data),
                Created = created
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

            if (dto.CompetitionIds != null)
            {
                foreach (var cId in dto.CompetitionIds)
                {
                    DbContext.CompetitionLogs.Add(new CompetitionLog
                    {
                        Event = dto.Event,
                        Log = log,
                        CompetitionId = cId
                    });
                }
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

        public void SendLog(int profileId, LogEvents logEvent, ITemplateEmailDTO dto)
        {
            return;
        }

        public GridData<LogGridDTO> GetGridData(LogGridParams gridParams)
        {
            return null;
        }

        #endregion


    }
}
