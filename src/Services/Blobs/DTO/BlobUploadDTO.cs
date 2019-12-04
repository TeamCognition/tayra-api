using Microsoft.AspNetCore.Http;
using Tayra.Common;

namespace Tayra.Services
{
    public class BlobUploadDTO
    {
        public BlobTypes BlobType { get; set; }
        public BlobPurposes BlobPurpose { get; set; }
        public IFormFile File { get; set; }
    }
}
