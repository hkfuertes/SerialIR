using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SerialIR.Common
{
    class Configuration
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("remote")]
        public string Remote { get; set; }

        [JsonProperty("keys")]
        public Dictionary<string, string> Keys { get; set; }
    }
}
