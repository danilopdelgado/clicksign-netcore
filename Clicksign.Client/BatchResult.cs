using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Clicksign.Client
{
    public class BatchResult
    {
        public string Key { get; set; }

        [JsonProperty(PropertyName = "signer_key")]
        public string SignerKey { get; set; }

        [JsonProperty(PropertyName = "document_keys")]
        public string[] DocumentKeys { get; set; }

        public bool Summary { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime Created { get; set; }
        
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime Updated { get; set; }
    }
}
