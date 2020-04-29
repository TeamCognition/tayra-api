using Microsoft.AspNetCore.Http;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IBlobsService
    {
        Blob UploadToAzure(BlobUploadDTO dto);
    }
}
