using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
    public class SignerRequest
    {
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phone_number")]
        public string PhoneNumber { get; set; }

        public string[] Auths { get; set; }

        public string Name { get; set; }

        public string Documentation { get; set; }

        public string Birthday { get; set; }

        [JsonProperty(PropertyName = "has_documentation")]
        public bool HasDocumentation { get; set; }

        public string Delivery { get; set; }
    }
}
