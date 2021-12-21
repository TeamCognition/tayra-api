﻿using System;
using System.Collections.Generic;
using Cog.DAL;

namespace Tayra.Models.Organizations
{
    public class PullRequestReview : EntityGuidId, ITimeStampedEntity
    {
        public PullRequestReview()
        {
            Comments = new List<PullRequestReviewComment>();
        }

        public string Body { get; set; }

        public string State { get; set; }

        public string CommitId { get; set; }

        public DateTime SubmittedAt { get; set; }

        public Guid PullRequestId { get; set; }
        public PullRequest PullRequest { get; set; }

        public string ReviewExternalId { get; set; }

        public Guid? ReviewerProfileId { get; set; }

        public Profile ReviewerProfile { get; set; }

        #region ITimeStampedEntity

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public ICollection<PullRequestReviewComment> Comments { get; set; }

        #endregion
    }
}