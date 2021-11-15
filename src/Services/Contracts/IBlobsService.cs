using Microsoft.AspNetCore.Http;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public interface IBlobsService
    {
        Blob UploadToAzure(BlobUpload msg);
    }
}
