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

        public Dictionary<string, string> Claims { get; set; }
        
        /// <summary>
        /// This is just an extra security measure. When a user provides a claim, the hash also needs to be signed
        /// by the user's private key for it to be valid, so this isn't technically needed.
        /// This is just to provide extra confidence about having this stored on the blockchain.
        /// (Even though only the hash of this class will be stored on the block)
        /// </summary>
        public DateTime RenewalDate { get; set; }
    }
}
