using System;
using System.Collections.Generic;

namespace Data
{
    public class Claim
    {
        public DateTime Expires { get; set; }

        public string PubKey { get; set; }

        public Dictionary<string, string> Claims { get; set; }
    }
}
