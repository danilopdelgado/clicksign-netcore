using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clicksign.Client
{
    public class DocumentResult
    {  
        public DocumentInfo Document { get; set; }
        
        public class DocumentInfo
        {
            public DocumentInfo()
            {
                this.Downloads = new DownloadUrls();
                this.Signers = new List<Signer>();
                this.Events = new List<Event>();
            }

            public string Key { get; set; }

            public string Path { get; set; }

            [JsonProperty(PropertyName = "filename")]
            public string FileName { get; set; }
            
            [JsonProperty(PropertyName = "uploaded_at")]
            public DateTime Uploaded { get; set; }
            
            [JsonProperty(PropertyName = "updated_at")]
            public DateTime? Updated { get; set; }
            
            [JsonProperty(PropertyName = "finished_at")]
            public DateTime? Finished { get; set; }
            
            [JsonProperty(PropertyName = "deadline_at")]
            public DateTime? Deadline { get; set; }

            public string Status { get; set; }
            
            [JsonProperty(PropertyName = "auto_close")]
            public bool AutoClose { get; set; }

            public string Locale { get; set; }

            public DownloadUrls Downloads { get; set; }

            public IList<Signer> Signers { get; set; }

            public IList<Event> Events { get; set; }
        }

        public class Signer
        {
            public string Key { get; set; } 

            [JsonProperty(PropertyName = "list_key")]
            public string ListKey { get; set; }

            [JsonProperty(PropertyName = "request_signature_key")]
            public string RequestSignatureKey { get; set; }

            public string Email { get; set; }

            [JsonProperty(PropertyName = "sign_as")]
            public SignerSign Sign { get; set; }

            public string[] Auths { get; set; }

            [JsonProperty(PropertyName = "phone_number")]
            public string PhoneNumber { get; set; }

            public string Name { get; set; }

            public string Documentation { get; set; }

            public DateTime? Birthday { get; set; }

            [JsonProperty(PropertyName = "has_documentation")]
            public bool HasDocumentation { get; set; }

            public string Delivery { get; set; }

            [JsonProperty(PropertyName = "created_at")]
            public DateTime Created { get; set; }

            public SignerSignature Signature { get; set; }
        }

        public class DownloadUrls
        {
            [JsonProperty(PropertyName = "original_file_url")]
            public string OriginalFileUrl { get; set; }

            [JsonProperty(PropertyName = "signed_file_url")]
            public string SignedFileUrl { get; set; }

            [JsonProperty(PropertyName = "ziped_file_url")]
            public string ZipedFileUrl { get; set; }
        }

        public class SignerSignature
        {
            public string Name { get; set; }

            public string Email { get; set; }

            public DateTime? Birthday { get; set; }

            public string Documentation { get; set; }

            public SignerSignatureValidation Validation { get; set; }

            [JsonProperty(PropertyName = "ip_address")]
            public string IpAddress { get; set; }
        }

        public class SignerSignatureValidation
        {
            public string Status { get; set; }
            public string Name { get; set; }
        }

        public class Event
        {
            public string Name { get; set; }

            public dynamic Data { get; set; }

            [JsonProperty(PropertyName = "occurred_at")]
            public DateTime Occurred { get; set; }
        }
    }
}