using System;
using System.Linq;
using Firdaws.Core;
using Firdaws.DAL;
using Newtonsoft.Json;
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

            if(dto.ProfileId.HasValue)
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

        public GridData<LogGridDTO> GetGridData(LogGridParams gridParams)
        {
            IQueryable<LogGridDTO> query;
            if (gridParams.ProfileId.HasValue)
            {
                query = from l in DbContext.ProfileLogs
                        where l.ProfileId == gridParams.ProfileId
                        select new LogGridDTO
                        {
                            Data = JsonConvert.DeserializeObject(l.Log.Data),
                            Event = l.Event,
                            Created = l.Log.Created
                        };
            }
            else if(gridParams.CompetitionId.HasValue)
            {
                query = from l in DbContext.CompetitionLogs
                        where l.CompetitionId == gridParams.CompetitionId
                        select new LogGridDTO
                        {
                            Data = JsonConvert.DeserializeObject(l.Log.Data),
                            Event = l.Event,
                            Created = l.Log.Created
                        };
            }
            else if(gridParams.ShopId.HasValue)
            {
                query = from l in DbContext.ShopLogs
                        where l.ShopId == gridParams.ShopId
                        select new LogGridDTO
                        {
                            Data = JsonConvert.DeserializeObject(l.Log.Data),
                            Event = l.Event,
                            Created = l.Log.Created
                        };
            }
            else
            {
                throw new ApplicationException("provide profileId or competitiondId");
            }

            GridData<LogGridDTO> gridData = query.GetGridData(gridParams);

            return gridData;
        }

        #endregion


    }
}
