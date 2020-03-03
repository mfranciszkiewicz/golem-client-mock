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
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GolemMarketMockAPI.MarketAPI.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Demand : DemandOfferBase, IEquatable<Demand>
    { 
        /// <summary>
        /// Gets or Sets DemandId
        /// </summary>
        [DataMember(Name="demandId")]
        public string DemandId { get; set; }

        /// <summary>
        /// Gets or Sets RequestorId
        /// </summary>
        [DataMember(Name="requestorId")]
        public string RequestorId { get; private set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Demand {\n");
            sb.Append("  DemandId: ").Append(DemandId).Append("\n");
            sb.Append("  RequestorId: ").Append(RequestorId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public  new string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Demand)obj);
        }

        /// <summary>
        /// Returns true if Demand instances are equal
        /// </summary>
        /// <param name="other">Instance of Demand to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Demand other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    DemandId == other.DemandId ||
                    DemandId != null &&
                    DemandId.Equals(other.DemandId)
                ) && 
                (
                    RequestorId == other.RequestorId ||
                    RequestorId != null &&
                    RequestorId.Equals(other.RequestorId)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (DemandId != null)
                    hashCode = hashCode * 59 + DemandId.GetHashCode();
                    if (RequestorId != null)
                    hashCode = hashCode * 59 + RequestorId.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Demand left, Demand right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Demand left, Demand right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
