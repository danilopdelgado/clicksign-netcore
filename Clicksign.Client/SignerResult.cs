using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
    public class SignerResult
    {
        public string Key { get; set; }

        public string Email { get; set; }
        
        public string[] Auths { get; set; }

        public string Name { get; set; }

        public string Documentation { get; set; }

        public DateTime? Birthday { get; set; }

        [JsonProperty(PropertyName = "phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "has_documentation")]
        public bool HasDocumentation { get; set; }
        
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? Created { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? Updated { get; set; }
    }
}
