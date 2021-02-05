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
            public string Id { get; init; }
        }

        //IRequestHandler
        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly OrganizationDbContext _db;
            private readonly IConfiguration _config;

            public Handler(OrganizationDbContext db, IConfiguration config)
            {
                _db = db;
                _config = config;
            }

            public async Task<Result> Handle(Command command, CancellationToken token)
            {
                var isImage = ImageFileExtensions.Any(ex =>
                    command.File.FileName.EndsWith(ex, StringComparison.OrdinalIgnoreCase));
                if (!isImage)
                {
                    throw new ApplicationException("File has to be an image");
                }

                var blob = new BlobsService(_config, _db).UploadToAzure(new BlobUploadDTO
                {
                    File = command.File,
                    BlobType = command.BlobType,
                    BlobPurpose = command.BlobPurpose
                });
                await _db.SaveChangesAsync(token);

                return new Result
                {
                    Id = $"{_config["ImagerServer"]}{_config["BlobContainerImages"]}/{blob.Id}.{blob.Extension}"
                };
            }
        }
    }
}