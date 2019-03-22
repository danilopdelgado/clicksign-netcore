using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
   public class DocumentSignerRequest
    {
        [JsonProperty(PropertyName = "document_key")]
        public string DocumentKey { get; set; }

        [JsonProperty(PropertyName = "signer_key")]
        public string SignerKey { get; set; }

        [JsonProperty(PropertyName = "sign_as")]
        public string Sign { get; set; }
    }
}
