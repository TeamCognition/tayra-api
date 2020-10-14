﻿using System;

namespace Tayra.Models.Organizations
{
    public class PullRequestReviewComment
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }
        public string Body { get; set; }
        
        public string ExternalUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public int? UserCommentedPullRequestReviewProfileId { get; set; }

        public Profile UserCommentedPullRequestReviewProfile { get; set; }

        public int PullRequestReviewId { get; set; }
        
        public PullRequestReview PullRequestReview { get; set; }

        public int PullRequestId { get; set; }  
        
        public PullRequest PullRequest { get; set; }
    }
}