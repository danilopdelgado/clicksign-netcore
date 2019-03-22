using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
    public class DocumentRequest
    {
        public string Path { get; set; }
        
        [JsonProperty(PropertyName = "content_base64")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "deadline_at")]
        public string Deadline { get; set; }

        [JsonProperty(PropertyName = "auto_close")]
        public bool AutoClose { get; set; }

        public string Locale { get; set; }
    }
}
