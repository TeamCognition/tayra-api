﻿using System;

namespace Tayra.Services
{
    public class QuestCommitsGridDTO
    {
        public Guid ProfileId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public DateTime CommittedOn { get; set; }
    }
}
