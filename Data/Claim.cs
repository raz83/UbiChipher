using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// This is the multi purpose class that is hashed and written to the blockchain,
    /// but also used to transfer the same data over HTTPS so that the service requesting
    /// these details can also confirm the content.
    /// </summary>
    public class Claim
    {
        public string PubKey { get; set; }

        /// <summary>
        /// NOTE: We should attempt to only use one key value pair per Claim,
        /// so that a user can proof each claim independently. But this will not be enforce.
        /// If they want a collection, bundled together, they can. This is why this is a
        /// Dictionary and not a KeyValuePair.
        /// 
        /// TODO: Consider changing this to KeyValuePair to actually simply to approach??
        /// </summary>
        public Dictionary<string, string> ClaimPairs { get; set; } 

        /// <summary>
        /// KYC services can't be 100% sure. So uncertainty needs to be captured.
        /// Especially if there is a chain of trust involved, as uncertainty factors can accumulate.
        /// Validators can then set their own acceptable threshold values.
        /// </summary>
        public decimal Certainty { get; set; } 

        /// <summary>
        /// This is just an extra security measure. When a user provides a claim, the hash also needs to be signed
        /// by the user's private key for it to be valid, so this isn't technically needed.
        /// This is just to provide extra confidence about having this stored on the blockchain.
        /// (Even though only the hash of this class will be stored on the block)
        /// </summary>
        public DateTime RenewalDate { get; set; }
    }
}
