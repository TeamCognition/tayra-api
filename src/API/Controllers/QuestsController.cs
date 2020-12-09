using System;
using Cog.Core;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class QuestsController : BaseController
    {
        #region Constructor

        public QuestsController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider)
        {
            OrganizationContext = context;
        }

        #endregion

        public OrganizationDbContext OrganizationContext { get; set; }

        #region Action Methods

        [HttpPost("search")]
        public ActionResult<GridData<QuestViewGridDTO>> GetQuestsGrid([FromBody] QuestViewGridParams gridParams)
        {
            return QuestsService.GetQuestsGrid(CurrentUser.SegmentsIds, gridParams);
        }

        [HttpPost("searchCommits")]
        public ActionResult<GridData<QuestCommitsGridDTO>> GetQuestCommits([FromBody] QuestCommitsGridParams gridParams)
        {
            return QuestsService.GetQuestCommitsGrid(CurrentUser.ProfileId, gridParams);
        }

        [HttpPost("searchCompletitions")]
        public ActionResult<GridData<QuestCompletitionsGridDTO>> GetQuestCompletitions([FromBody] QuestCompletitionsGridParams gridParams)
        {
            return QuestsService.GetQuestCompletitionsGrid(CurrentUser.ProfileId, gridParams);
        }

        [HttpGet("{questId:int}")]
        public ActionResult<QuestViewDTO> GetSegmentQuests([FromRoute] int questId)
        {
            return QuestsService.GetQuestViewDTO(CurrentUser.ProfileId, questId);
        }

        [HttpPost("create")]
        public IActionResult CreateQuest([FromBody] QuestCreateDTO dto)
        {
            QuestsService.Create(dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPut("update/{questId:int}")]
        public IActionResult UpdateQuest([FromRoute] int questId, [FromBody] QuestUpdateDTO dto)
        {
            QuestsService.Update(questId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("completeGoal")]
        public IActionResult CompleteGoal([FromBody] QuestGoalCompleteDTO dto)
        {
            QuestsService.CompleteGoal(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("complete")]
        public IActionResult CompleteQuest([FromBody] QuestCompleteDTO dto)
        {
            QuestsService.CompleteQuest(dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("commit")]
        public IActionResult CommitToQuest([FromBody] QuestCommitDTO dto)
        {
            QuestsService.CommitToQuest(CurrentUser.ProfileId, dto);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        [HttpPost("end/{questId}")]
        public IActionResult EndQuest([FromRoute] int questId)
        {
            QuestsService.EndQuest(CurrentUser.ProfileId, questId);
            OrganizationContext.SaveChanges();

            return Ok();
        }

        #endregion
    }
}