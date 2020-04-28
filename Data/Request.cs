using System;
using System.Collections.Generic;
using System.Text;

namespace UbiChipher.Data
{
    public class Request
    {
        public List<string> ClaimRequests { get; set; }

        public string PostBackUri { get; set; }
    }
}
