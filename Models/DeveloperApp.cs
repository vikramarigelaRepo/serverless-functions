
using System.Collections.Generic;

       
    public class DeveloperApp
    {
        public string AppFamily { get; set; }
        public string AppId { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string CallbackUrl { get; set; }
        public long CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<Credential> Credentials { get; set; }
        public string DeveloperId { get; set; }
        public long LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public string Name { get; set; }
        public List<object> Scopes { get; set; }
        public string Status { get; set; }
    }