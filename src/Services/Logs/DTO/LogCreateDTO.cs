using System;
using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Services
{
    public record LogCreateDTO
    {
        public LogCreateDTO(LogEvents eventType,  DateTime timestamp, string description, string externalUrl, Dictionary<string, string> data, Guid? profileId, Guid? shopId = null)
        {
            Event = eventType;
            Description = description;
            ExternalUrl = externalUrl;
            Data = data;
            ProfileId = profileId;
            ShopId = shopId;
        }
        
        public LogEvents Event { get; init; }
        public string Description { get; init; }
        public string ExternalUrl { get; init; }
        public Dictionary<string, string> Data { get; init; }
        public DateTime Timestamp { get; init; }
        public Guid? ProfileId { get; init; }
        public Guid? ShopId { get; init; }
    }
}
