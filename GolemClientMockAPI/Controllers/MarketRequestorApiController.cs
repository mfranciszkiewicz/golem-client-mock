/*
 * Yagna Market API
 *
 *  ## Yagna Market The Yagna Market is a core component of the Yagna Network, which enables computational Offers and Demands circulation. The Market is open for all entities willing to buy computations (Demands) or monetize computational resources (Offers). ## Yagna Market API The Yagna Market API is the entry to the Yagna Market through which Requestors and Providers can publish their Demands and Offers respectively, find matching counterparty, conduct negotiations and make an agreement.  This version of Market API conforms with capability level 1 of the <a href=\"https://docs.google.com/document/d/1Zny_vfgWV-hcsKS7P-Kdr3Fb0dwfl-6T_cYKVQ9mkNg\"> Market API specification</a>.  Market API contains two roles: Requestors and Providers which are symmetrical most of the time (excluding agreement phase). 
 *
 * OpenAPI spec version: 1.4.2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using GolemClientMockAPI.Attributes;
using GolemMarketMockAPI.MarketAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using GolemClientMockAPI.Repository;
using GolemClientMockAPI.Processors;
using GolemClientMockAPI.Mappers;
using GolemClientMockAPI.Security;

namespace GolemMarketMockAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [GolemClientAuthorizationFilter(DefaultNodeId = "DummyRequestorNodeId")]
    //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
    public class MarketRequestorApiController : Controller
    {
        public IRequestorMarketProcessor MarketProcessor { get; set; }
        public ISubscriptionRepository SubscriptionRepository { get; set; }
        public IProposalRepository ProposalRepository { get; set; }
        public IAgreementRepository AgreementRepository { get; set; }

        public MarketRequestorEventMapper RequestorEventMapper { get; set; }
        public DemandMapper DemandMapper { get; set; }
        public OfferMapper OfferMapper { get; set; }

        public MarketRequestorApiController(IRequestorMarketProcessor marketProcessor,
            ISubscriptionRepository subscriptionRepository,
            IProposalRepository proposalRepository,
            IAgreementRepository agreementRepository,
            MarketRequestorEventMapper requestorEventMapper,
            DemandMapper demandMapper,
            OfferMapper offerMapper)
        {
            this.MarketProcessor = marketProcessor;
            this.SubscriptionRepository = subscriptionRepository;
            this.ProposalRepository = proposalRepository;
            this.AgreementRepository = agreementRepository;
            this.RequestorEventMapper = requestorEventMapper;
            this.DemandMapper = demandMapper;
            this.OfferMapper = offerMapper;
        }

        /// <summary>
        /// Cancels agreement.
        /// </summary>
        /// <remarks>Causes the awaiting &#x60;waitForApproval&#x60; call to return with &#x60;Cancelled&#x60; response. </remarks>
        /// <param name="agreementId"></param>
        /// <response code="204">Agreement cancelled.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="409">Agreement already approved.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpDelete]
        [Route("/market-api/v1/agreements/{agreementId}")]
        [ValidateModelState]
        [SwaggerOperation("CancelAgreement")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual async Task<IActionResult> CancelAgreement([FromRoute][Required]string agreementId)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            // locate the agreement
            var agreement = this.AgreementRepository.GetAgreement(agreementId);

            if (agreement == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            if (clientContext.NodeId != agreement.DemandProposal.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }

            var result = await this.MarketProcessor.CancelAgreement(agreementId);

            if(result) // if cancel successful
            {
                return StatusCode(204);
            }
            else
            {
                return StatusCode(409, new Error() { });  // HTTP 409 Conflict
            }
            
        }

        /// <summary>
        /// Reads Market responses to published Demand.
        /// </summary>
        /// <remarks>This is a blocking operation. It will not return until there is at least one new event.  **Note**: When &#x60;collectOffers&#x60; is waiting, simultaneous call to &#x60;unsubscribeDemand&#x60; on the same &#x60;subscriptionId&#x60; should result in \&quot;Subscription does not exist\&quot; error returned from &#x60;collectOffers&#x60;.  **Note**: Specification requires this endpoint to support list of specific Proposal Ids to listen for messages related only to specific Proposals. This is not covered yet. </remarks>
        /// <param name="subscriptionId"></param>
        /// <param name="timeout"></param>
        /// <param name="maxEvents"></param>
        /// <response code="200">Proposal event list.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpGet]
        [Route("/market-api/v1/demands/{subscriptionId}/events")]
        [ValidateModelState]
        [SwaggerOperation("CollectOffers")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Event>), description: "Proposal event list.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual async Task<IActionResult> CollectOffers([FromRoute][Required]string subscriptionId, [FromQuery]int? timeout, [FromQuery]int? maxEvents)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            var subscription = this.SubscriptionRepository.GetDemandSubscription(subscriptionId);

            if(subscription == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            if(clientContext.NodeId != subscription.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }
            
            var events = await this.MarketProcessor.CollectRequestorEventsAsync(subscriptionId, timeout, (int?)maxEvents);

            var result = events.Select(proposal => this.RequestorEventMapper.Map(proposal))
                               .ToList();

            // Return the collected requestor events (including offer proposals)
            return StatusCode(200, result);
        }

        /// <summary>
        /// Sends Agreement draft to the Provider.
        /// </summary>
        /// <remarks>Signs Agreement self-created via &#x60;createAgreement&#x60; and sends it to the Provider. </remarks>
        /// <param name="agreementId"></param>
        /// <response code="204">Agreement confirmed.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="410">Agreement cancelled.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpPost]
        [Route("/market-api/v1/agreements/{agreementId}/confirm")]
        [ValidateModelState]
        [SwaggerOperation("ConfirmAgreement")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult ConfirmAgreement([FromRoute][Required]string agreementId)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            // locate the agreement
            var agreement = this.AgreementRepository.GetAgreement(agreementId);

            if (agreement == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            if (clientContext.NodeId != agreement.DemandProposal.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }

            this.MarketProcessor.SendConfirmAgreement(agreementId);

            return StatusCode(200);
        }

        /// <summary>
        /// Creates Agreement from selected Proposal.
        /// </summary>
        /// <remarks>Initiates the Agreement handshake phase.  Formulates an Agreement artifact from the Proposal indicated by the received Proposal Id.  The Approval Expiry Date is added to Agreement artifact and implies the effective timeout on the whole Agreement Confirmation sequence.  A successful call to &#x60;createAgreement&#x60; shall immediately be followed by a &#x60;confirmAgreement&#x60; and &#x60;waitForApproval&#x60; call in order to listen for responses from the Provider.  **Note**: Moves given Proposal to &#x60;Approved&#x60; state. </remarks>
        /// <param name="body"></param>
        /// <response code="201">Agreement created.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpPost]
        [Route("/market-api/v1/agreements")]
        [ValidateModelState]
        [SwaggerOperation("CreateAgreement")]
        [SwaggerResponse(statusCode: 201, type: typeof(string), description: "Agreement created.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "Bad request.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult CreateAgreement([FromBody]AgreementProposal agreement)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            // locate the offerProposalId
            var offerProposal = this.ProposalRepository.GetOfferProposal(agreement.ProposalId);

            var receivingSubscription = this.SubscriptionRepository.GetDemandSubscription(offerProposal.ReceivingSubscriptionId);

            if (offerProposal == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            if (clientContext.NodeId != receivingSubscription.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }

            var resultAgreement = this.MarketProcessor.CreateAgreement(agreement.ProposalId, agreement.ValidTo);

            return StatusCode(201, resultAgreement.Id);

        }

        /// <summary>
        /// Responds with a bespoke Demand to received Offer.
        /// </summary>
        /// <remarks>Creates and sends a modified version of original Demand (a counter-proposal) adjusted to previously received Proposal (ie. Offer). Changes Proposal state to &#x60;Draft&#x60;. Returns created Proposal id. </remarks>
        /// <param name="body"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="proposalId"></param>
        /// <response code="201">Counter Proposal created.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="410">Proposal rejected.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpPost]
        [Route("/market-api/v1/demands/{subscriptionId}/proposals/{proposalId}")]
        [ValidateModelState]
        [SwaggerOperation("CreateProposalDemand")]
        [SwaggerResponse(statusCode: 201, type: typeof(string), description: "Counter Proposal created.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "Bad request.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult CreateProposalDemand([FromBody]Proposal demandProposal, [FromRoute][Required]string subscriptionId, [FromRoute][Required]string proposalId)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            var subscription = this.SubscriptionRepository.GetDemandSubscription(subscriptionId);

            if (subscription == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            if (clientContext.NodeId != subscription.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }

            var demandEntity = new GolemClientMockAPI.Entities.Demand()
                {
                    NodeId = clientContext.NodeId,
                    Constraints = demandProposal.Constraints,
                    Properties = demandProposal.Properties as Dictionary<string, string>
                };

            try
            {
                var demandProposalEntity = this.MarketProcessor.CreateDemandProposal(subscriptionId, proposalId, demandEntity);

                return StatusCode(201, demandProposalEntity.Id);
            }
            catch (Exception exc)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }
        }

        /// <summary>
        /// Fetches all active Demands which have been published by the Requestor.
        /// </summary>
        /// <response code="200">Demand list.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpGet]
        [Route("/market-api/v1/demands")]
        [ValidateModelState]
        [SwaggerOperation("GetDemands")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<>), description: "Demand list.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "Bad request.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult GetDemands()
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            var subscriptions = this.SubscriptionRepository.GetActiveDemandSubscriptions(clientContext.NodeId);

            try
            {
                var demands = subscriptions.Select(subs => this.DemandMapper.MapEntityToModel(subs));

                return StatusCode(201, demands);
            }
            catch (Exception exc)
            {
                return StatusCode(0, new Error() { }); // unexpecetd error
            }
        }

        /// <summary>
        /// Fetches Proposal (Offer) with given id.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="proposalId"></param>
        /// <response code="200">Proposal.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="410">Proposal rejected.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpGet]
        [Route("/market-api/v1/demands/{subscriptionId}/proposals/{proposalId}")]
        [ValidateModelState]
        [SwaggerOperation("GetProposalOffer")]
        [SwaggerResponse(statusCode: 200, type: typeof(Proposal), description: "Proposal.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult GetProposalOffer([FromRoute][Required]string subscriptionId, [FromRoute][Required]string proposalId)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            var subscription = this.SubscriptionRepository.GetDemandSubscription(subscriptionId);

            if (subscription == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            if (clientContext.NodeId != subscription.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }

            var offerProposal = this.ProposalRepository.GetOfferProposals(subscriptionId).Where(prop => prop.Id == proposalId).FirstOrDefault();

            if(offerProposal == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            var demandProposal = (offerProposal.DemandId == null) ? 
                                    new GolemClientMockAPI.Entities.DemandProposal() { Id = subscriptionId, Demand = subscription.Demand }  : 
                                    this.ProposalRepository.GetDemandProposal(offerProposal.DemandId);

            var result = this.OfferMapper.MapEntityToProposal(offerProposal);
            
            return StatusCode(200, result);
        }

        /// <summary>
        /// Handles dynamic property query.
        /// </summary>
        /// <remarks>The Market Matching Mechanism, when resolving the match relation for the specific Demand-Offer pair, is to detect the “dynamic” properties required (via constraints) by the other side. At this point, it is able to query the issuing node for those properties and submit the other side’s requested properties as the context of the query.  **Note**: The property query responses may be submitted in “chunks”, ie. the responder may choose to resolve ‘quick’/lightweight’ properties faster and provide response sooner, while still working on more time-consuming properties in the background. Therefore the response contains both the resolved properties, as well as list of properties which responder knows still require resolution.  **Note**: This method must be implemented for Market API Capability Level 2. </remarks>
        /// <param name="body"></param>
        /// <param name="subscriptionId"></param>
        /// <param name="queryId"></param>
        /// <response code="204">OK, query reply posted.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpPost]
        [Route("/market-api/v1/demands/{subscriptionId}/propertyQuery/{queryId}")]
        [ValidateModelState]
        [SwaggerOperation("PostQueryReplyDemands")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "Bad request.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult PostQueryReplyDemands([FromBody]PropertyQueryReply body, [FromRoute][Required]string subscriptionId, [FromRoute][Required]string queryId)
        { 
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400, default(Error));

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404, default(Error));

            //TODO: Uncomment the next line to return response 0 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(0, default(Error));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Rejects Proposal (Offer).
        /// </summary>
        /// <remarks>Effectively ends a Negotiation chain - it explicitly indicates that the sender will not create another counter-Proposal. </remarks>
        /// <param name="subscriptionId"></param>
        /// <param name="proposalId"></param>
        /// <response code="204">Proposal rejected.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="410">Proposal already rejected.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpDelete]
        [Route("/market-api/v1/demands/{subscriptionId}/proposals/{proposalId}")]
        [ValidateModelState]
        [SwaggerOperation("RejectProposalOffer")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult RejectProposalOffer([FromRoute][Required]string subscriptionId, [FromRoute][Required]string proposalId)
        { 
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);

            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401, default(Error));

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404, default(Error));

            //TODO: Uncomment the next line to return response 410 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(410);

            //TODO: Uncomment the next line to return response 0 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(0, default(Error));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Publishes Requestor capabilities via Demand.
        /// </summary>
        /// <remarks>Demand object can be considered an \&quot;open\&quot; or public Demand, as it is not directed at a specific Provider, but rather is sent to the market so that the matching mechanism implementation can associate relevant Offers.  **Note**: it is an \&quot;atomic\&quot; operation, ie. as soon as Subscription is placed, the Demand is published on the market. </remarks>
        /// <param name="body"></param>
        /// <response code="201">Subscribed.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpPost]
        [Route("/market-api/v1/demands")]
        [ValidateModelState]
        [SwaggerOperation("SubscribeDemand")]
        [SwaggerResponse(statusCode: 201, type: typeof(string), description: "Subscribed.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Error), description: "Bad request.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult SubscribeDemand([FromBody]Demand body)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            var demandEntity = this.DemandMapper.MapToEntity(body);

            demandEntity.NodeId = clientContext.NodeId;

            var subscription = this.MarketProcessor.SubscribeDemand(demandEntity);

            // return created Subscription Id
            return StatusCode(201, subscription.Id);
        }

        /// <summary>
        /// Stop subscription for previously published Demand.
        /// </summary>
        /// <remarks>Stop receiving Proposals.  **Note**: this will terminate all pending &#x60;collectOffers&#x60; calls on this subscription. This implies, that client code should not &#x60;unsubscribeDemand&#x60; before it has received all expected/useful inputs from &#x60;collectOffers&#x60;. </remarks>

        /// <param name="subscriptionId"></param>
        /// <response code="204">Demand revoked.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="410">Already unsubscribed.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpDelete]
        [Route("/market-api/v1/demands/{subscriptionId}")]
        [ValidateModelState]
        [SwaggerOperation("UnsubscribeDemand")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public virtual IActionResult UnsubscribeDemand([FromRoute][Required]string subscriptionId)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            var subscription = this.SubscriptionRepository.GetDemandSubscription(subscriptionId);

            if (subscription == null)
            {
                return StatusCode(404, new Error() { } ); // Not Found
            }

            if (clientContext.NodeId != subscription.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }

            this.MarketProcessor.UnsubscribeDemand(subscriptionId);

            return StatusCode(204);
        }

        /// <summary>
        /// Waits for Agreement approval by the Provider.
        /// </summary>
        /// <remarks>This is a blocking operation. The call may be aborted by Requestor caller code. After the call is aborted, another &#x60;waitForApproval&#x60; call can be raised on the same Agreement Id.  It returns one of the following options: * &#x60;Ok&#x60; - Indicates that the Agreement has been approved by the Provider.   - The Provider is now ready to accept a request to start an Activity     as described in the negotiated agreement.   - The Requestor’s corresponding &#x60;waitForApproval&#x60; call returns Ok after     this on the Provider side.  * &#x60;Rejected&#x60; - Indicates that the Provider has called &#x60;rejectAgreement&#x60;,   which effectively stops the Agreement handshake. The Requestor may attempt   to return to the Negotiation phase by sending a new Proposal.  * &#x60;Cancelled&#x60; - Indicates that the Requestor himself has called  &#x60;cancelAgreement&#x60;, which effectively stops the Agreement handshake. </remarks>
        /// <param name="agreementId"></param>
        /// <param name="timeout"></param>
        /// <response code="200">Agreement approval result.</response>
        /// <response code="401">Authorization information is missing or invalid.</response>
        /// <response code="404">The specified resource was not found.</response>
        /// <response code="0">Unexpected error.</response>
        [HttpPost]
        [Route("/market-api/v1/agreements/{agreementId}/wait")]
        [ValidateModelState]
        [SwaggerOperation("WaitForApproval")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Agreement approval result.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Error), description: "Authorization information is missing or invalid.")]
        [SwaggerResponse(statusCode: 404, type: typeof(Error), description: "The specified resource was not found.")]
        [SwaggerResponse(statusCode: 0, type: typeof(Error), description: "Unexpected error.")]
        public async virtual Task<IActionResult> WaitForApproval([FromRoute][Required]string agreementId, [FromQuery]int? timeout)
        {
            var clientContext = this.HttpContext.Items["ClientContext"] as GolemClientMockAPI.Entities.ClientContext;

            // locate the agreement
            var agreement = this.AgreementRepository.GetAgreement(agreementId);

            if (agreement == null)
            {
                return StatusCode(404, new Error() { }); // Not Found
            }

            if (clientContext.NodeId != agreement.DemandProposal.Demand.NodeId)
            {
                return StatusCode(401, new Error() { }); // Unauthorized
            }


            var result = await this.MarketProcessor.WaitConfirmAgreementResponseAsync(agreementId, 10000);

            switch (result)
            {
                case GolemClientMockAPI.Entities.AgreementResultEnum.Approved:
                case GolemClientMockAPI.Entities.AgreementResultEnum.Rejected:
                case GolemClientMockAPI.Entities.AgreementResultEnum.Cancelled:
                case GolemClientMockAPI.Entities.AgreementResultEnum.Timeout:
                default:
                    return StatusCode(200, result.ToString());
            }
            
        }
    }
}
