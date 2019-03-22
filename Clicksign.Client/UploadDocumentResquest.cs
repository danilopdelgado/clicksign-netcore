using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clicksign.Client
{
    public class UploadDocumentResquest
    {
        [JsonProperty(PropertyName = "document")]
        public DocumentRequest Document { get; set; }
    }
}
