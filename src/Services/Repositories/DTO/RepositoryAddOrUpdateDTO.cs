using System;

namespace Tayra.Services.Repositories.DTO
{
    public class RepositoryAddOrUpdateDTO
    {
        public Guid? TeamId { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }
        public string NameWithOwner { get; set; }
        public string PrimaryLanguage { get; set; }
        public string ExternalUrl { get; set; }
    }
}