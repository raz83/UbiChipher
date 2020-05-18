//using BitcoinLib.Services.Coins.Base;
//using BitcoinLib.Services.Coins.Bitcoin;
using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UbiChipher.Utilities;

namespace UbiChipher.Infrastructure.Blockchain
{
    public class BlockchainClaimHashFinder : IBlockchainClaimHashFinder
    {
        //private readonly ICoinService CoinService = new BitcoinService(useTestnet: true);

        public BlockchainClaimHashFinder()
        {
            CreateTestData();
        }

        public string GetClientClaimFingerPrintFromBlockchain(Claim claim)
        {
            // This will check the hash the client provided against the hash stored on the blockchain. 

            return testFingerprint;

            // TODO: complete this implementation

            //var transactionToScanForClaim = CoinService.ListTransactions(clientPubKey);

        }

        #region Test Data
        //Claim testClaim;
        string testFingerprint;

        private void CreateTestData()
        {
            var testClaim = new Claim() //[{"Expires":"2021-04-21T15:24:12.6242835Z","PubKey":"21489122-ae06-4bb6-b01b-6e46bb15cc64","Claims":{"Name":"Murray"}}]
            {
                ClaimPairs = new Dictionary<string, string>()
                   {
                       { "Name", "Murray" }
                   },

                RenewalDate = DateTime.Parse("2021-04-21T15:24:12.6242835Z"), // DateTime.UtcNow.AddDays(365),

                PubKey = "21489122-ae06-4bb6-b01b-6e46bb15cc64" //Guid.NewGuid().ToString()
            };

            var testClaimsList = new List<Claim>() { testClaim };

            var original = JsonConvert.SerializeObject(testClaimsList);

            testFingerprint = Cryptography.CreateMD5(original);
        }

        #endregion
    }
}
