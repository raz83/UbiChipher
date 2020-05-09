using Data;
using System;
using System.Collections.Generic;

namespace UbiChipher.Data
{
    public class ClaimsEnvelope
    {
        /// <summary>
        /// This is to ensure backwards compatibility with older versions of this protocol. 
        /// It is neccessary so that the protocol can get any sercurity patches that it may need.
        /// </summary>
        public string ProtocalVersoin { get; set; }

        /// <summary>
        /// The Claims.
        /// </summary>
        public List<Claim> Claims { get; set; }

        /// <summary>
        /// This is the name of the orginiation that is being authorized to use this claim collection.
        /// </summary>
        public string IssuedTo { get; set; }

        /// <summary>
        /// The time this was issued.
        /// </summary>
        public DateTime IssuedAt { get; set; }

        /// <summary>
        /// NOTE: THIS EXPIRATION ISN'T A SECURITY FEATURE!!!
        /// <seealso cref="Claim.RenewalDate"/>
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}
