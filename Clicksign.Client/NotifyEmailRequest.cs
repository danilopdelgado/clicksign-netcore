using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
    public class NotifyEmailRequest
    {
        [JsonProperty(PropertyName = "request_signature_key")]
        public string RequestSignatureKey { get; set; }

        public string Message { get; set; }

        public string Url { get; set; }
    }
}
