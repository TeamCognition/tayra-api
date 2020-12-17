using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tayra.Common;
using Tayra.Models.Organizations;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Profile
{
    public partial class ProfilesController
    {
        [HttpGet("notification/settings")]
        public async Task<UpdateNotificationSettings.Command> GetNotificationSettings()
            => await _mediator.Send(new UpdateNotificationSettings.Query {ProfileId = CurrentUser.ProfileId});
        
        
        [HttpPut("notification/settings")]
        public async Task UpdateNotificationSettings([FromBody] UpdateNotificationSettings.Command command)
            => await _mediator.Send(command with { ProfileId = CurrentUser.ProfileId});
        
    }
    public class UpdateNotificationSettings
    {
        public record Query : IRequest<Command>
        {
            public Guid ProfileId { get; init; }
        }

        public record Command : IRequest
        {
            public Guid ProfileId { get; init; }
            public Setting[] Settings { get; set; }

            public record Setting
            {
                public LogEvents LogEvent { get; init; }
                public bool IsEnabled { get; init; }
            }
        }
        
        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly OrganizationDbContext _db;

            public QueryHandler(OrganizationDbContext db) => _db = db;

            public async Task<Command> Handle(Query msg, CancellationToken token)
            {
                var devices = await _db.LogDevices.Where(x => x.ProfileId == msg.ProfileId && x.Type == LogDeviceTypes.Email).Include(x => x.Settings).FirstOrDefaultAsync(token);

                return new Command
                {
                    Settings = devices.Settings.Select(x => new Command.Setting
                    {
                        LogEvent = x.LogEvent,
                        IsEnabled = x.IsEnabled
                    }).ToArray()
                };
            }
        }
        
        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly OrganizationDbContext _db;

            public CommandHandler(OrganizationDbContext db) => _db = db;

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var devices = await _db.LogDevices.Where(x => x.ProfileId == msg.ProfileId && x.Type == LogDeviceTypes.Email).Include(x => x.Settings).ToArrayAsync(token);

                foreach (var d in devices)
                {
                    foreach (var sdto in msg.Settings)
                    {
                        var s = d.Settings.FirstOrDefault(x => x.LogEvent == sdto.LogEvent);
                        if (s == null)
                        {
                            s = new LogSetting
                            {
                                ProfileId = msg.ProfileId,
                                LogDeviceId = d.Id,
                                LogEvent = sdto.LogEvent
                            };
                            d.Settings.Add(s);
                        }
                        s.IsEnabled = sdto.IsEnabled;
                    }
                }

                await _db.SaveChangesAsync(token);
            }
        }
    }
}