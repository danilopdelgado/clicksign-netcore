using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clicksign.Client
{
    public class ErrorResult
    {
        [JsonProperty("errors")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> Errors { get; set; }
    }
}
