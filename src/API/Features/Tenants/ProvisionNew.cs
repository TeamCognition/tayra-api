using System;
using System.Linq;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tayra.API.Features.Segments;
using Tayra.Common;
using Tayra.Mailer;
using Tayra.Models.Catalog;
using Tayra.Models.Organizations;
using Tayra.Models.Seeder;
using Tayra.Services;
using Task = System.Threading.Tasks.Task;

namespace Tayra.API.Features.Tenants
{
    public partial class TenantsController
    {
        [AllowAnonymous, HttpPost("provisionNew")]
        public async Task ProvisionNew([FromBody] ProvisionNew.Command command)
            => await _mediator.Send(command);
    }
    
    public class ProvisionNew
    {
        public record Command : IRequest
        {
            public string EmailAddress { get; set; }
            public string Identifier { get; init; }
            public string DisplayName { get; init; }
            public string Timezone { get; init; }
        }
        
        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly CatalogDbContext _catalogDb;
            private readonly IMailerService _mailerService;
            private readonly IConfiguration _config;
            public Handler(CatalogDbContext catalogDb, IMailerService mailerService, IConfiguration config)
            {
                _catalogDb = catalogDb;
                _mailerService = mailerService;
                _config = config;
            }

            protected override async Task Handle(Command msg, CancellationToken token)
            {
                var tenantDbName = $"tayra_tenant-{msg.Identifier.Split('.').First()}";
                var newDbSqlConnection = new SqlConnectionStringBuilder
                {
                    InitialCatalog = tenantDbName,
                    DataSource = _config["CatalogServer"],
                    UserID = _config["DatabaseUser"],
                    Password = _config["DatabasePassword"],
                    Encrypt = true,
                    TrustServerCertificate = true //for aws retardness
                }.ToString();

                var tenant = new Tenant
                {
                    Id = Guid.NewGuid().ToString(),
                    Identifier = msg.Identifier,
                    Name = msg.DisplayName,
                    Timezone = msg.Timezone,
                    ConnectionString = newDbSqlConnection
                };
                
                await EnsureTenantCreatedAndInvitedAsync(_catalogDb, newDbSqlConnection, tenant, msg.EmailAddress, _mailerService, token);

                _catalogDb.Add(tenant);
                await _catalogDb.SaveChangesAsync(token);
            }

            private static async Task EnsureTenantCreatedAndInvitedAsync(CatalogDbContext catalogDb, string connectionString, Tenant tenant, string emailAddress, IMailerService mailerService, CancellationToken token)
            {
                await using var newDb = new OrganizationDbContext(TenantModel.WithConnectionStringOnly(connectionString), null);
                await newDb.Database.MigrateAsync(token);

                await using var tenantDb = new OrganizationDbContext(new TenantModel(tenant.Id, tenant.Identifier, tenant.Name, 
                    tenant.ConnectionString, tenant.Timezone, tenant.ServicePlan), null);
                
                await tenantDb.LocalTenants.AddAsync(new LocalTenant
                {
                    TenantId = Guid.Parse(tenant.Id),
                    DisplayName = tenant.Name,
                    Identifier = tenant.Identifier
                }, token);

                EssentialSeeds.AddEssentialSeeds(tenantDb);
                await new Create.Handler(tenantDb).HandleHack(new Create.Command
                {
                    Name = "Segment 1",
                    Key = "S1"
                }, token);
                
                await tenantDb.SaveChangesAsync(token);

                var segmentId = tenantDb.Segments.Select(x => x.Id).FirstOrDefault();
                var teamId = tenantDb.Teams.Select(x => x.Id).FirstOrDefault();
                
                await new Tayra.Services._Models.Identities.IdentitiesService().SendInvitation(tenantDb, catalogDb,
                    tenant.Identifier, new IdentityInviteDTO
                    {
                        Role = ProfileRoles.Admin,
                        EmailAddress = emailAddress,
                        SegmentId = segmentId,
                        TeamId = teamId
                    },
                    mailerService);

                await tenantDb.SaveChangesAsync(token);
            }
        }      
    }
}