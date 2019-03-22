using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
    public class DocumentSignerResult
    {
        public string Key { get; set; }

        [JsonProperty(PropertyName = "document_key")]
        public string DocumentKey { get; set; }

        [JsonProperty(PropertyName = "signer_key")]
        public string SignerKey { get; set; }

        [JsonProperty(PropertyName = "sign_as")]
        public string Sign { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime? Created { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? Updated { get; set; }
    }
}
