﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tayra.Common;
using Tayra.Models.Organizations;
using Tayra.Services;

namespace Tayra.API.Controllers
{
    public class LookupsController : BaseController
    {
        #region Constructor

        public LookupsController(IServiceProvider serviceProvider, OrganizationDbContext context) : base(serviceProvider)
        {
            OrganizationContext = context;
        }

        #endregion

        public OrganizationDbContext OrganizationContext { get; set; }

        #region From Enum


        [HttpGet, Route(nameof(LookupTypes.TokenTypes))]
        public IEnumerable<LookupDTO> GetTokenTypes()
        {
            return LookupsService.GetFromEnum<TokenType>();
        }

        [HttpGet, Route(nameof(LookupTypes.ExperienceRanks))]
        public IEnumerable<LookupDTO> GetExperienceRanks()
        {
            return LookupsService.GetFromEnum<ExperienceRanks>();
        }

        [HttpGet, Route(nameof(LookupTypes.CompetitionStatuses))]
        public IEnumerable<LookupDTO> GetCompetitionStatuses()
        {
            return LookupsService.GetFromEnum<CompetitionStatus>();
        }

        [HttpGet, Route(nameof(LookupTypes.ItemTypes))]
        public IEnumerable<LookupDTO> GetItemTypes()
        {
            return LookupsService.GetFromEnum<ItemTypes>();
        }

        [HttpGet, Route(nameof(LookupTypes.ItemRarities))]
        public IEnumerable<LookupDTO> GetItemRarities()
        {
            return LookupsService.GetFromEnum<ItemRarities>();
        }

        [HttpGet, Route(nameof(LookupTypes.ProfileRoles))]
        public IEnumerable<LookupDTO> GetProfileRoles()
        {
            return LookupsService.GetFromEnum<ProfileRoles>();
        }

        [HttpGet, Route(nameof(LookupTypes.ShopPurchaseStatuses))]
        public IEnumerable<LookupDTO> GetShopPurchaseStatuses()
        {
            return LookupsService.GetFromEnum<ShopPurchaseStatuses>();
        }

        #endregion

        #region From Database

        [HttpGet, Route(nameof(LookupTypes.Tokens))]
        public IEnumerable<LookupDTO> GetTokens()
        {
            return LookupsService.GetFromOrganizationDb<Token>(x => new LookupDTO(x.Id, x.Name));
        }

        #endregion

        #region Bulk Lookup

        [HttpGet, Route("")]
        public Dictionary<LookupTypes, IEnumerable<LookupDTO>> Get([FromQuery(Name = "types")]IEnumerable<LookupTypes> types)
        {
            var result = new Dictionary<LookupTypes, IEnumerable<LookupDTO>>();

            foreach (var type in types)
            {
                result[type] =
                    (type == LookupTypes.Tokens) ? GetTokens()
                    : (type == LookupTypes.TokenTypes) ? GetTokenTypes()
                    : (type == LookupTypes.ExperienceRanks) ? GetExperienceRanks()
                    : (type == LookupTypes.CompetitionStatuses) ? GetCompetitionStatuses()
                    : (type == LookupTypes.ItemTypes) ? GetItemTypes()
                    : (type == LookupTypes.ItemRarities) ? GetItemRarities()
                    : (type == LookupTypes.ProfileRoles) ? GetProfileRoles()
                    : (type == LookupTypes.ShopPurchaseStatuses) ? GetShopPurchaseStatuses()
                    : null;
            }

            return result;
        }

        #endregion
    }
}