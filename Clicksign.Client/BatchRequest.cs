using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
    public class BatchRequest
    {
        public BatchRequest()
        {
            this.Summary = true;
        }

        [JsonProperty(PropertyName = "document_keys")]
        public string[] DocumentKeys { get; set; }

        [JsonProperty(PropertyName = "signer_key")]
        public string SignerKey { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public bool Summary { get; set; }
    }
}
