using System.Collections.Generic;

namespace UbiChipher.Data
{
    public class Request
    {
        public List<string> ClaimRequests { get; set; }

        public string PostBackUri { get; set; }
    }
}
