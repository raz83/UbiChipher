using Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UbiChipher.Infrastructure.Blockchain;
using UbiChipher.Utilities;

namespace UbiChipher.Services
{
    public class ClaimValidationService
    {
        IBlockchainClaimHashFinder blockchainClaimHashFinder;

        public ClaimValidationService(IBlockchainClaimHashFinder blockchainClaimHashFinder)
        {
            // TODO: Use dependency injection, then do (software engineering stuff...):
            //this.blockchainClaimHashFinder = blockchainClaimHashFinder ?? throw new ArgumentNullException(nameof(blockchainClaimHashFinder));

            // TODO: Remove once DI added (actually add this DI stuff everywhere in the solution, then add unit tests, stuff...
            this.blockchainClaimHashFinder = new BlockchainClaimHashFinder();
        }

        public void ValidateClaim(string textFromRESTApi, out string hashOfClient, out string hashOnBlockChain, out bool match)
        {
            // TODO: Should deserialize from SighnedClaimsEnvelope first instead, checking user signature, request detail etc.
            var clientClaims = JsonConvert.DeserializeObject<List<Claim>>(textFromRESTApi).Where(x => x.ClaimPairs.Any(y => y.Key == "Name")).ToList();//.Single();
            //var clientPubKey = clientClaim.PubKey;

            //TODO: This will need to be changed to iterate over SighnedClaimsEnvelope.ClaimsEnvelope.Claims 
            {
                hashOfClient = Cryptography.CreateMD5(textFromRESTApi); //TODO: change textFromRESTApi to SighnedClaimsEnvelope.ClaimsEnvelope.Claims iteration.
                hashOnBlockChain = this.blockchainClaimHashFinder.GetClientClaimFingerPrintFromBlockchain(clientClaims.Single()); //TODO: change to SighnedClaimsEnvelope.ClaimsEnvelope.Claims iteration.
            }

            match = hashOfClient == hashOnBlockChain;
        }
    }
}
