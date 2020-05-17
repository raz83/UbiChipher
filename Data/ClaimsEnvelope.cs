using Data;
using System;
using System.Collections.Generic;

namespace UbiChipher.Data
{
    class SighnedClaimsEnvelope
    {
        /// <summary>
        /// This is signature using the private key that matches the corresponding public key in Claim.PubKey
        /// It is needed so that the receiver of the ClaimsEnvelope data can be sure the user has indented for the validator 
        /// to use it for a specific request, at a specific time and for a specific reason.
        /// </summary>
        public string Signature { get; set; }

        public ClaimsEnvelope ClaimsEnvelope { get; set; }
    }

    public class ClaimsEnvelope
    {
        /// <summary>
        /// This is to ensure backwards compatibility with older versions of this protocol. 
        /// It is necessary so that the protocol can get any security patches that it may need.
        /// </summary>
        public string ProtocalVersoin { get; set; }


        /// <summary>
        /// The Claims.
        /// </summary>
        public List<Claim> Claims { get; set; }

        /// <summary>
        /// This is the name of the organization that is being authorized to use this claim collection.
        /// </summary>
        public string IssuedTo { get; set; }

        /// <summary>
        /// Used by the validator that check that the response is 
        /// specifically for the reason requested by the validator.
        /// </summary>
        public string RequestIdentifier { get; set; }

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
