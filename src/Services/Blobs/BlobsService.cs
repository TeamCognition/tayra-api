using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Tayra.Common;
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

        public Blob UploadToAzure(BlobUpload msg)
        {
            var blob = new Blob
            {
                Id = Guid.NewGuid(),
                Filesize = msg.File.Length,
                Extension = GetExtension(msg.File.FileName),
                Filename = Path.GetFileNameWithoutExtension(msg.File.FileName),
                Type = msg.BlobType,
                Purpose = msg.BlobPurpose
            };

            CloudBlockBlob blockBlob = _imageContainer.GetBlockBlobReference($"{blob.Id.ToString()}.{blob.Extension}");
            blockBlob.Properties.ContentType = msg.File.ContentType;
            blockBlob.Properties.ContentDisposition = $"attachment;filename=\"{msg.BlobPurpose}-{blob.Id}\"";

            using (Stream stream = msg.File.OpenReadStream())
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
