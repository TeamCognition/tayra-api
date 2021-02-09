using Microsoft.AspNetCore.Http;
using Tayra.Common;

namespace Tayra.Services
{
    public record BlobUpload
    { 
        public BlobTypes BlobType { get; init; }
        public BlobPurposes BlobPurpose { get; init; }
        public IFormFile File { get; init; }
    }
}