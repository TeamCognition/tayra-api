using System;
using System.Linq;
using Firdaws.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class BlobsController : BaseController
    {
        private readonly static string[] ImageFileExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

        #region Constructor

        public BlobsController(OrganizationDbContext dbContext, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Properties

        private readonly OrganizationDbContext DbContext;

        #endregion

        #region Action Methods

        [HttpPost("upload")]
        public ActionResult<IDTO> Create([FromForm] BlobUploadDTO dto)
        {
            var isImage = ImageFileExtensions.Any(ex => dto.File.FileName.EndsWith(ex, StringComparison.OrdinalIgnoreCase));

            if(!isImage)
            {
                return BadRequest("file has to be an image");
            }

            var blob = BlobsService.UploadToAzureAndSave(dto);
            DbContext.SaveChanges();

            return Ok(new { Id = $"{blob.Id}.{blob.Extension}" });
        }

        #endregion
    }
}