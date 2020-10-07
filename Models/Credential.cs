using System.Collections.Generic;

public class Credential
    {
        public List<ApiProduct> ApiProducts { get; set; }
        public List<object> Attributes { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public long ExpiresAt { get; set; }
        public long IssuedAt { get; set; }
        public List<object> Scopes { get; set; }
        public string Status { get; set; }
    }