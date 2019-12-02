using System;
using System.IO;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Tayra.Models.Organizations;

namespace Tayra.Services
{
    public class BlobsService : BaseService<OrganizationDbContext>, IBlobsService
    {
        private readonly IConfiguration _config;

        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _storageClient;
        private readonly CloudBlobContainer _imageContainer;

        #region Constructor

        public BlobsService(IConfiguration config, OrganizationDbContext dbContext) : base(dbContext)
        {
            _config = config;

            _storageAccount = new CloudStorageAccount(new StorageCredentials("tayra", "98ZragdvKWY2WgKDOsKTZfhXze0nAe8/wkZKzOanlcF7W9qrjqwX/HMg6upmZ9c3Sgu9FvvGyfh1N+zb1gtPVA=="), true);
            _storageClient = _storageAccount.CreateCloudBlobClient();
            _imageContainer = _storageClient.GetContainerReference("imgs");
        }

        public Blob UploadToAzureAndSave(BlobUploadDTO dto)
        {
            var blob = new Blob
            {
                Id = Guid.NewGuid(),
                Filesize = dto.File.Length,
                Extension = GetExtension(dto.File.FileName),
                Filename = Path.GetFileNameWithoutExtension(dto.File.FileName),
                Type = dto.BlobType,
                Purpose = dto.BlobPurpose
            };

            CloudBlockBlob blockBlob = _imageContainer.GetBlockBlobReference(blob.Id.ToString());
            blockBlob.Properties.ContentType = dto.File.ContentType;
            blockBlob.Properties.ContentDisposition = $"attachment;filename=\"{dto.BlobPurpose}-{blob.Id}\"";

            using (Stream stream = dto.File.OpenReadStream())
            {
                stream.Position = 0;
                blockBlob.UploadFromStream(stream);
            }

            return DbContext.Add(blob)
                .Entity;
        }

        #endregion

        private string GetExtension(string filename)
        {
            var extension = Path.GetExtension(filename);
            if (!string.IsNullOrWhiteSpace(extension))
                return extension.Substring(1);

            return "UNKNOWN";
        }
    }
}
