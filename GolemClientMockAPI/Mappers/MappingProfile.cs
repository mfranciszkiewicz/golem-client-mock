﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ...from Entities

            CreateMap<Entities.RequestorEvent, GolemMarketMockAPI.MarketAPI.Models.RequestorEvent>()
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateRequestorEventType(src.EventType)));

            CreateMap<Entities.RequestorEvent, GolemMarketMockAPI.MarketAPI.Models.OfferEvent>()
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateRequestorEventType(src.EventType)))
                    .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.OfferProposal.Offer.NodeId))
                    .ForMember(dest => dest.Offer, opt => opt.MapFrom(src => src.OfferProposal));

            CreateMap<Entities.ProviderEvent, GolemMarketMockAPI.MarketAPI.Models.ProviderEvent>()
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateProviderEventType(src.EventType)));
            CreateMap<Entities.ProviderEvent, GolemMarketMockAPI.MarketAPI.Models.DemandEvent>()
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateProviderEventType(src.EventType)))
                    .ForMember(dest => dest.RequestorId, opt => opt.MapFrom(src => src.DemandProposal.Demand.NodeId))
                    .ForMember(dest => dest.Demand, opt => opt.MapFrom(src => src.DemandProposal));
            CreateMap<Entities.ProviderEvent, GolemMarketMockAPI.MarketAPI.Models.NewAgreementEvent>()
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateProviderEventType(src.EventType)))
                    .ForMember(dest => dest.AgreementId, opt => opt.MapFrom(src => src.Agreement.Id))
                    .ForMember(dest => dest.RequestorId, opt => opt.MapFrom(src => src.Agreement.Demand.NodeId))
                    .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.Agreement.Offer.NodeId))
                    .ForMember(dest => dest.Offer, opt => opt.MapFrom(src => src.Agreement.Offer))
                    .ForMember(dest => dest.Demand, opt => opt.MapFrom(src => src.Agreement.Demand));


            CreateMap<Entities.Demand, GolemMarketMockAPI.MarketAPI.Models.Demand>();
            CreateMap<Entities.DemandProposal, GolemMarketMockAPI.MarketAPI.Models.Proposal>()
                .ForMember(dest => dest.PrevProposalId, opt => opt.MapFrom(src => src.OfferId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Constraints, opt => opt.MapFrom(src => src.Demand.Constraints))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Demand.Properties));

            CreateMap<Entities.Offer, GolemMarketMockAPI.MarketAPI.Models.Offer>();
            CreateMap<Entities.OfferProposal, GolemMarketMockAPI.MarketAPI.Models.Proposal>()
                .ForMember(dest => dest.PrevProposalId, opt => opt.MapFrom(src => src.DemandId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Constraints, opt => opt.MapFrom(src => src.Offer.Constraints))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Offer.Properties));

            // ...to Entities

            CreateMap<GolemMarketMockAPI.MarketAPI.Models.Demand, Entities.Demand>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => PropertyMappers.MapFromJsonString(src.Properties.ToString())));

            CreateMap<GolemMarketMockAPI.MarketAPI.Models.Offer, Entities.Offer>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => PropertyMappers.MapFromJsonString(src.Properties.ToString())));

        }
    }
}