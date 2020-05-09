using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UbiChipher.Services
{
    public class ClaimValidationService
    {
        //private readonly ICoinService CoinService = new BitcoinService(useTestnet: true);

        public ClaimValidationService()
        {
            CreateTestData();
        }

        public void ValidateClaim(string textFromRESTApi, out string hashOfClient, out string hashOnBlockChain, out bool match)
        {
            var clientClaim = JsonConvert.DeserializeObject<List<Claim>>(textFromRESTApi).Where(x => x.Claims.Any(y => y.Key == "Name")).Single();
            var clientPubKey = clientClaim.PubKey;

            hashOfClient = CreateMD5(textFromRESTApi);
            hashOnBlockChain = GetClientClaimFingerPrintFromBlockchain(clientPubKey, hashOfClient);

            match = hashOfClient == hashOnBlockChain;
        }

        private string GetClientClaimFingerPrintFromBlockchain(string clientPubKey, string hashOfClient)
        {
            // This will check the hash the client provided against the hash stored on the blockchain.

            return testFingerprint;

            // TODO: complete this implementation

            //var transactionToScanForClaim = CoinService.ListTransactions(clientPubKey);

        }

        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
     
        }



        #region Test Data
        //Claim testClaim;
        string testFingerprint;

        private void CreateTestData()
        {
            var testClaim = new Claim() //[{"Expires":"2021-04-21T15:24:12.6242835Z","PubKey":"21489122-ae06-4bb6-b01b-6e46bb15cc64","Claims":{"Name":"Murray"}}]
            {
                Claims = new Dictionary<string, string>()
                   {
                       { "Name", "Murray" }
                   },

                RenewalDate = DateTime.Parse("2021-04-21T15:24:12.6242835Z"), // DateTime.UtcNow.AddDays(365),

                PubKey = "21489122-ae06-4bb6-b01b-6e46bb15cc64" //Guid.NewGuid().ToString()
            };

            var testClaimsList = new List<Claim>() { testClaim };

            var original = JsonConvert.SerializeObject(testClaimsList);

            testFingerprint = CreateMD5(original);
        }

        #endregion
    }
}
