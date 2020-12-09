﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tayra.Models.Organizations;

namespace Tayra.API.Controllers
{
    public class OnboardingController : BaseController
    {
        #region Constructor
        public OnboardingController(IServiceProvider serviceProvider, OrganizationDbContext dbContext) : base(serviceProvider)
        {
            OrganizationContext = dbContext;
        }

        #endregion
        
        #region Properties

        protected OrganizationDbContext OrganizationContext;

        #endregion
        
        #region Action Methods

        [HttpPost("completeStep")]

        public IActionResult OnboardingSteps([FromBody] OnboardingStepIds step)
        {
            
            var o = OrganizationContext.Organizations.FirstOrDefault();
            var p = OrganizationContext.Profiles.Where(x => x.Id == CurrentUser.ProfileId).FirstOrDefault();
            MarkStepAsCompleted(step, o, p);
            OrganizationContext.SaveChanges();
            return Ok();
        }
        
        public enum OnboardingStepIds 
        {
            CreateProfile,
            CreateSegment,
            AddSources,
            InviteUsers
        }
        
        public void MarkStepAsCompleted(OnboardingStepIds s, Organization o, Profile p)
        {
            switch (s)
            {
                case OnboardingStepIds.CreateProfile:
                    p.isCreateProfileOnboarding = true;
                    break;
                case OnboardingStepIds.CreateSegment:
                    o.isCreateSegmentOnboarding = true;
                    break;
                case OnboardingStepIds.AddSources:
                    o.isAddSourcesOnboarding = true;
                    break;
                case OnboardingStepIds.InviteUsers:
                    o.isInviteUsersOnboarding = true;
                    break;
            }
        }
        
        public class OnboardingDto
        {
            public Dictionary<string, bool> Steps { get; set; }
        }

        [HttpGet("completedSteps")]
        public ActionResult<OnboardingDto> GetCompletedSteps()
        {
            var isCreateProfileCompleted = OrganizationContext.Profiles.Where(x => x.Id == CurrentUser.ProfileId)
                .Select(x => 
                    x.isCreateProfileOnboarding
                ).FirstOrDefault();
            
            var completedOnboardingSteps = OrganizationContext.Organizations
                .Select(x => new 
                {
                    IsCreateSegmentCompleted = x.isCreateSegmentOnboarding,
                    IsAddSourcesCompleted = x.isAddSourcesOnboarding,
                    IsInviteUsersCompleted = x.isAddSourcesOnboarding
                }).FirstOrDefault();


            return new OnboardingDto
            {
                Steps = new Dictionary<string, bool>()
                {
                    {OnboardingStepIds.CreateProfile.ToString(), isCreateProfileCompleted},
                    {OnboardingStepIds.CreateSegment.ToString(), completedOnboardingSteps.IsCreateSegmentCompleted},
                    {OnboardingStepIds.AddSources.ToString(), completedOnboardingSteps.IsAddSourcesCompleted},
                    {OnboardingStepIds.InviteUsers.ToString(), completedOnboardingSteps.IsInviteUsersCompleted }
                }
            };
        }
        #endregion
        
    }
}