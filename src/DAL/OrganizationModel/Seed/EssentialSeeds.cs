using System.Collections.Generic;
using Tayra.Common;

namespace Tayra.Models.Organizations
{
    public static class EssentialSeeds
    {
        public static void AddEssentialSeeds(OrganizationDbContext dbContext)
        {
            var tokensSeed = new List<Token>
            {
                new Token { Id = 1, Name = "Company Token", Symbol = "CT", Type = TokenType.CompanyToken },
                new Token { Id = 2, Symbol = "EXP", Name = nameof(TokenType.Experience), Type = TokenType.Experience },
                new Token { Id = 3, Symbol = "1Up", Name = nameof(TokenType.OneUp), Type = TokenType.OneUp }
            };

            var shopsSeed = new List<Shop>
            {
                new Shop { Id = 1, Name = "Employee Shop" }
            };

            var taskCategorySeed = new List<TaskCategory>
            {
                new TaskCategory { Id = 1, Name = "Undefined" }
            };

            dbContext.AddRange(tokensSeed);
            dbContext.AddRange(shopsSeed);
            dbContext.AddRange(taskCategorySeed);
        }
    }
}
