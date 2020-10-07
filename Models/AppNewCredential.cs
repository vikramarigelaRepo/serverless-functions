using System.Collections.Generic;
using Newtonsoft.Json;

public class AppNewCrendential
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("apiProducts")]
        public List<string> ApiProducts { get; set; }

        [JsonProperty("keyExpiresIn")]
        public long KeyExpiresIn { get; set; }
    }
