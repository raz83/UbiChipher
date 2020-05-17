using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace UbiChipher.Data
{
    public class Enrolment
    {
        public List<Claim>  Claims { get; set; }

        public Intermediary Intermediary { get; set; }
    }

    public class Intermediary
    {
        /// <summary>
        /// Intermediaries can be a recursive tree.
        /// BUT THEY SHOUND NOT!! Adds security and uncertainty risks.
        /// </summary>
        public Intermediary Child { get; set; }


        /// <summary>
        /// Name of the intermediary
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID of the intermediary
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Digital certificate that UbiChiper issues to enrollment provided ensure data
        /// comes from trusted source.
        /// </summary>
        public string Certificate { get; set; }

        /// <summary>
        /// How much UbiChipher trust this enrollment provider (is the site run by an ASBO or not?)
        /// </summary>
        public decimal Certainty { get; set; }
    }
}
