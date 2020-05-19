using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SerialIR
{
    class Configuration
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("remote")]
        public string Remote { get; set; }

        [JsonProperty("keys")]
        public Dictionary<string, int> Keys { get; set; }
    }
}
