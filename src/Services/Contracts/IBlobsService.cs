using Microsoft.AspNetCore.Http;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IBlobsService
    {
        Blob UploadToAzureAndSave(BlobUploadDTO dto);
    }
}
