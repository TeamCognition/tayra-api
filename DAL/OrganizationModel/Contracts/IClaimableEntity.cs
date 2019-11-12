using System;

namespace Tayra.Models.Organizations
{
    public interface IClaimableEntity
    {
        bool ClaimRequired { get; set; }
        DateTime? ClaimedAt { get; set; }
    }
}