using AutoMapper;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Mappers;
using GolemMarketApiMockup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GolemMarketApiMockupTests
{
    [TestClass]
    public class RequestorEventMapperTests
    {

        public IMapper Mapper { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            this.Mapper = config.CreateMapper();
        }

        [TestMethod]
        public void Map_RequestorEvent_ToOfferEvent()
        {

            var mapper = new RequestorEventMapper(this.Mapper);

            var entity = new RequestorEvent()
            {
                EventType = RequestorEvent.RequestorEventType.Proposal,
                OfferProposal = new OfferProposal()
                {
                    DemandId = "PreviousDemandId",
                    InternalId = 2,
                    Offer = new Offer()
                    {
                        NodeId = "ProviderNodeId",
                        Constraints = "()",
                        Properties = new Dictionary<string, string>() { }
                    }
                }
            };

            var result = mapper.Map(entity) as GolemMarketMockAPI.MarketAPI.Models.OfferEvent;

            Assert.AreEqual(entity.OfferProposal.Offer.Constraints, result.Offer.Constraints);
            Assert.AreEqual(entity.OfferProposal.DemandId, result.Offer.PrevProposalId);
            Assert.AreEqual(entity.OfferProposal.Id, result.Offer.Id);
            Assert.AreEqual(entity.OfferProposal.Offer.NodeId, result.ProviderId);

        }



    }
}