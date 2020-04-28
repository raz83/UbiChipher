using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UbiChipher.Data;

namespace UbiChipher.Services
{
    public class ClaimGenerationService
    {
        List<Claim> claimsWallet;

        public ClaimGenerationService()
        {
            CreateTestData();
        }

        public string GenerateClaim(string QRText)
        {
            var pharesedQR = JsonConvert.DeserializeObject<Request>(QRText);

            var matches = claimsWallet.Where(x => x.Claims.Keys.Any(y => pharesedQR.ClaimRequests.Contains(y))).ToList();

            var postbackContent = JsonConvert.SerializeObject(matches);
            return postbackContent;
        }

        private void CreateTestData()
        {
            claimsWallet = new List<Claim>();


            var testClaim = new Claim() //[{"Expires":"2021-04-21T15:24:12.6242835Z","PubKey":"21489122-ae06-4bb6-b01b-6e46bb15cc64","Claims":{"Name":"Murray"}}]
            {
                Claims = new Dictionary<string, string>()
                   {
                       { "Name", "Murray" }
                   },

                Expires = DateTime.Parse("2021-04-21T15:24:12.6242835Z"), // DateTime.UtcNow.AddDays(365),

                PubKey = "21489122-ae06-4bb6-b01b-6e46bb15cc64" //Guid.NewGuid().ToString()
            };

            claimsWallet.Add(testClaim);
        }
    }
}
