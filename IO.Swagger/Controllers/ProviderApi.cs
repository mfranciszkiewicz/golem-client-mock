/*
 * Golem Market API
 *
 * Market API
 *
 * OpenAPI spec version: 1.0.0
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
using IO.Swagger.Attributes;
using IO.Swagger.Models;

namespace IO.Swagger.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    public class ProviderApiController : Controller
    { 
        /// <summary>
        /// 
        /// </summary>
        
        /// <param name="agreementId"></param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/market-api/v1/agreements/{agreementId}/approve")]
        [ValidateModelState]
        [SwaggerOperation("ApproveAgreement")]
        public virtual IActionResult ApproveAgreement([FromRoute][Required]string agreementId)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);


            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        
        /// <param name="subscriptionId"></param>
        /// <param name="timeout"></param>
        /// <param name="maxEvents"></param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/market-api/v1/offers/{subscriptionId}/events")]
        [ValidateModelState]
        [SwaggerOperation("Collect")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Proposal>), description: "OK")]
        public virtual IActionResult Collect([FromRoute][Required]string subscriptionId, [FromQuery]float? timeout, [FromQuery]long? maxEvents)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<Proposal>));

            string exampleJson = null;
            exampleJson = "[ {\n  \"prevProposalId\" : \"prevProposalId\",\n  \"id\" : \"id\",\n  \"constraints\" : \"constraints\",\n  \"properties\" : \"{}\"\n}, {\n  \"prevProposalId\" : \"prevProposalId\",\n  \"id\" : \"id\",\n  \"constraints\" : \"constraints\",\n  \"properties\" : \"{}\"\n} ]";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<Proposal>>(exampleJson)
            : default(List<Proposal>);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Creates agreement proposal
        /// </summary>
        
        /// <param name="subscriptionId"></param>
        /// <param name="proposalId"></param>
        /// <param name="proposal"></param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/market-api/v1/offers/{subscriptionId}/proposals/{proposalId}/offer")]
        [ValidateModelState]
        [SwaggerOperation("CreateProposal")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "OK")]
        public virtual IActionResult CreateProposal([FromRoute][Required]string subscriptionId, [FromRoute][Required]string proposalId, [FromBody]Proposal proposal)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(string));

            string exampleJson = null;
            exampleJson = "\"\"";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<string>(exampleJson)
            : default(string);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// Fetches agreement proposal from proposal id.
        /// </summary>
        
        /// <param name="subscriptionId"></param>
        /// <param name="proposalId"></param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/market-api/v1/offers/{subscriptionId}/proposals/{proposalId}")]
        [ValidateModelState]
        [SwaggerOperation("GetProposal")]
        [SwaggerResponse(statusCode: 200, type: typeof(AgreementProposal), description: "OK")]
        public virtual IActionResult GetProposal([FromRoute][Required]string subscriptionId, [FromRoute][Required]string proposalId)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(AgreementProposal));

            string exampleJson = null;
            exampleJson = "{\n  \"offer\" : {\n    \"prevProposalId\" : \"prevProposalId\",\n    \"id\" : \"id\",\n    \"constraints\" : \"constraints\",\n    \"properties\" : \"{}\"\n  },\n  \"id\" : \"id\",\n  \"demand\" : {\n    \"prevProposalId\" : \"prevProposalId\",\n    \"id\" : \"id\",\n    \"constraints\" : \"constraints\",\n    \"properties\" : \"{}\"\n  }\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<AgreementProposal>(exampleJson)
            : default(AgreementProposal);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        
        /// <param name="subscriptionId"></param>
        /// <param name="queryId"></param>
        /// <param name="propertyValues"></param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/market-api/v1/offers/{subscriptionId}/propertyQuery/{queryId}")]
        [ValidateModelState]
        [SwaggerOperation("QueryResponse")]
        public virtual IActionResult QueryResponse([FromRoute][Required]string subscriptionId, [FromRoute][Required]string queryId, [FromBody]PropertyQueryResponse propertyValues)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);


            throw new NotImplementedException();
        }

        /// <summary>
        /// Rejects agreement.
        /// </summary>
        
        /// <param name="agreementId"></param>
        /// <response code="204">Agreement rejected</response>
        [HttpPost]
        [Route("/market-api/v1/agreements/{agreementId}/reject")]
        [ValidateModelState]
        [SwaggerOperation("RejectAgreement")]
        public virtual IActionResult RejectAgreement([FromRoute][Required]string agreementId)
        { 
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);


            throw new NotImplementedException();
        }

        /// <summary>
        /// Rejects offer
        /// </summary>
        
        /// <param name="subscriptionId"></param>
        /// <param name="proposalId"></param>
        /// <response code="204">OK</response>
        [HttpDelete]
        [Route("/market-api/v1/offers/{subscriptionId}/proposals/{proposalId}")]
        [ValidateModelState]
        [SwaggerOperation("RejectProposal")]
        public virtual IActionResult RejectProposal([FromRoute][Required]string subscriptionId, [FromRoute][Required]string proposalId)
        { 
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);


            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        
        /// <param name="body">Offer description</param>
        /// <response code="201">OK</response>
        /// <response code="400">Bad offer desciption</response>
        [HttpPost]
        [Route("/market-api/v1/offers")]
        [ValidateModelState]
        [SwaggerOperation("Subscribe")]
        [SwaggerResponse(statusCode: 201, type: typeof(string), description: "OK")]
        public virtual IActionResult Subscribe([FromBody]Offer body)
        { 
            //TODO: Uncomment the next line to return response 201 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(201, default(string));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            string exampleJson = null;
            exampleJson = "\"\"";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<string>(exampleJson)
            : default(string);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        
        /// <param name="subscriptionId"></param>
        /// <response code="204">Delete</response>
        /// <response code="404">Subscription not found</response>
        [HttpDelete]
        [Route("/market-api/v1/offers/{subscriptionId}")]
        [ValidateModelState]
        [SwaggerOperation("Unsubscribe")]
        public virtual IActionResult Unsubscribe([FromRoute][Required]string subscriptionId)
        { 
            //TODO: Uncomment the next line to return response 204 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(204);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);


            throw new NotImplementedException();
        }
    }
}
