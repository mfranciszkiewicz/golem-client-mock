/*
 * YagnaNet Hub
 *
 * Centralized hub for YagnaNet MK1
 *
 * OpenAPI spec version: 0.1.0
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

namespace GolemMarketMockAPI.GolemNetHubAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Message : IEquatable<Message>
    { 
        /// <summary>
        /// Gets or Sets RequestId
        /// </summary>
        [Required]
        [DataMember(Name="requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Destination address
        /// </summary>
        /// <value>Destination address</value>
        [Required]
        [DataMember(Name="destination")]
        public string Destination { get; set; }

        /// <summary>
        /// Binary data (base64)
        /// </summary>
        /// <value>Binary data (base64)</value>
        [Required]
        [DataMember(Name="payload")]
        public string Payload { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Message {\n");
            sb.Append("  RequestId: ").Append(RequestId).Append("\n");
            sb.Append("  Destination: ").Append(Destination).Append("\n");
            sb.Append("  Payload: ").Append(Payload).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
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
            return obj.GetType() == GetType() && Equals((Message)obj);
        }

        /// <summary>
        /// Returns true if Message instances are equal
        /// </summary>
        /// <param name="other">Instance of Message to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Message other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    RequestId == other.RequestId ||
                    RequestId != null &&
                    RequestId.Equals(other.RequestId)
                ) && 
                (
                    Destination == other.Destination ||
                    Destination != null &&
                    Destination.Equals(other.Destination)
                ) && 
                (
                    Payload == other.Payload ||
                    Payload != null &&
                    Payload.Equals(other.Payload)
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
                    if (RequestId != null)
                    hashCode = hashCode * 59 + RequestId.GetHashCode();
                    if (Destination != null)
                    hashCode = hashCode * 59 + Destination.GetHashCode();
                    if (Payload != null)
                    hashCode = hashCode * 59 + Payload.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Message left, Message right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Message left, Message right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
