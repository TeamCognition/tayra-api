using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Features.Blobs
{
    public partial class BlobsController
    {
        [HttpPost("upload")]
        public async Task<Upload.Result> UploadBlob([FromQuery] Upload.Command command)
            => await _mediator.Send(command);
    }

    public class Upload
    {
        private static readonly string[] ImageFileExtensions = {".jpg", ".jpeg", ".gif", ".png"};

        public record Command : IRequest<Result>
        {
            public BlobTypes BlobType { get; init; }
            public BlobPurposes BlobPurpose { get; init; }
            public IFormFile File { get; init; }
        }

        public record Result
        {
            public Uri Url { get; init; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly OrganizationDbContext _db;
            private readonly IConfiguration _config;

            public Handler(OrganizationDbContext db, IConfiguration config)
            {
                _db = db;
                _config = config;
            }

            public async Task<Result> Handle(Command msg, CancellationToken token)
            {
                var isImage = ImageFileExtensions.Any(ex =>
                    msg.File.FileName.EndsWith(ex, StringComparison.OrdinalIgnoreCase));
                if (!isImage)
                {
                    throw new ApplicationException("File has to be an image");
                }

                var blob = new BlobsService(_config, _db).UploadToAzure(msg.File, msg.BlobType, msg.BlobPurpose);
                await _db.SaveChangesAsync(token);

                return new Result
                {
                    Url = new Uri($"{_config["ImagerServer"]}{_config["BlobContainerImages"]}/{blob.Id}.{blob.Extension}")
                };
            }
        }
    }
}