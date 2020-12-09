﻿using System;
using System.Collections.Generic;
using Tayra.Common;
using Tayra.Models.Organizations;

namespace Tayra.Models.Seeder
{
    public static class EssentialSeeds
    {
        public static void AddEssentialSeeds(OrganizationDbContext dbContext)
        {
            var tokensSeed = new List<Token>
            {
                new Token { Name = "Company Token", Symbol = "CT", Type = TokenType.CompanyToken },
                new Token { Symbol = "EXP", Name = nameof(TokenType.Experience), Type = TokenType.Experience },
            };

            var shopsSeed = new List<Shop>
            {
                new Shop { Name = "Employee Shop" }
            };

            var taskCategorySeed = new List<TaskCategory> //this entity is shared between tenants/orgs
            {
                new TaskCategory { Name = "Undefined" }
            };
            dbContext.AddRange(taskCategorySeed);

            dbContext.AddRange(tokensSeed);
            dbContext.AddRange(shopsSeed);

            Console.WriteLine("Added essentials");

        }
    }
}
