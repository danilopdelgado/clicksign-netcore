using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Clicksign.Client
{
    /// <summary>
    /// Clicksign API, more information visit <see cref="https://developers.clicksign.com/docs/introducao-a-documentacao">Clicksign Rest API</see>
    /// </summary>
    public class ClicksignClient
    {
        private readonly string token;
        private readonly ILog log;
        private readonly string[] locales = new string[] { "pt-BR", "en-US" };

        static ClicksignClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        /// <summary>
        /// Initialize new instance of class <see cref="ClicksignClient"/>
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="token">Token</param>
        public ClicksignClient(string host, string token, CultureInfo cultureInfo = null)
        {
            cultureInfo = cultureInfo ?? Thread.CurrentThread.CurrentCulture;
            if (string.IsNullOrEmpty(host)) throw new ArgumentNullException("host", "Host is null or empty.");
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("token", "Token is null or empty.");
            if (!locales.Contains(cultureInfo.Name)) throw new ArgumentException("cultureInfo", string.Format("Allowed only cultures {0}.", string.Join(", ", locales)));

            this.Host = host;
            this.token = token;
            this.CultureInfo = cultureInfo;
            this.log = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Get Clicksign host
        /// </summary>
        public CultureInfo CultureInfo { get; private set; }

        /// <summary>
        /// Get Clicksign host
        /// </summary>
        public string Host { get; private set; }
        
        /// <summary>
        /// Upload file, more information visit <see cref="https://developers.clicksign.com/docs/criar-documento">Clicksign Rest API</see>
        /// </summary>
        public Task<DocumentResult> UploadDocument(string path, byte[] file, bool autoClose = true, DateTime? deadlineAt = null)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path", "File path is null or empty.");
            if (file.Length.Equals(0)) throw new ArgumentNullException("file", "File is empty.");
           
            string content = string.Format("data:{0};base64,{1}", MimeMapping.Get(System.IO.Path.GetFileName(path)), Convert.ToBase64String(file));

            var data = new UploadDocumentResquest
            {
                Document = new DocumentRequest()
                {
                    Path = path,
                    Content = content,
                    Deadline = deadlineAt.HasValue ? deadlineAt.Value.ToString("o") : null,
                    AutoClose = autoClose,
                    Locale = CultureInfo.Name
                }
            };

            return SendAsync<DocumentResult>(HttpMethod.Post, "api/v1/documents", data);
        }

        /// <summary>
        /// Get document, more information visit <see cref="https://developers.clicksign.com/docs/visualizar-documento">Clicksign Rest API</see>
        /// </summary>
        /// <returns><see cref="Document"/></returns>
        public Task<DocumentResult> GetDocument(string documentKey)
        {
            if (string.IsNullOrEmpty(documentKey)) throw new ArgumentNullException("documentKey", "Document Key is null or empty.");

            return SendAsync<DocumentResult>(HttpMethod.Get, string.Format("/api/v1/documents/{0}", documentKey));
        }

        /// <summary>
        /// Cancel <see cref="Document"/>, more information visit <see cref="https://developers.clicksign.com/docs/cancelar-documento">Clicksign Rest API</see>
        /// </summary>
        public Task<DocumentResult> CancelDocument(string documentKey)
        {
            if (string.IsNullOrEmpty(documentKey)) throw new ArgumentNullException("documentKey", "Document Key is null or empty.");

            return SendAsync<DocumentResult>(new HttpMethod("PATCH"), string.Format("/api/v1/documents/{0}/cancel", documentKey));
        }

        /// <summary>
        /// Finish <see cref="Document"/>, more information visit <see cref="https://developers.clicksign.com/docs/finalizar-documento">Clicksign Rest API</see>
        /// </summary>
        public Task<DocumentResult> FinishDocument(string documentKey)
        {
            if (string.IsNullOrEmpty(documentKey)) throw new ArgumentNullException("documentKey", "Document Key is null or empty.");

            return SendAsync<DocumentResult>(new HttpMethod("PATCH"), string.Format("/api/v1/documents/{0}/finish", documentKey));
        }

        /// <summary>
        /// Create signer <see cref="Document"/>, more information visit <see cref="https://developers.clicksign.com/docs/criar-signatario">Clicksign Rest API</see>
        /// </summary>
        public async Task<SignerResult> CreateSigner(string name, string document, DateTime birthday, string email, string phoneNumber = null, NotificationType notify = NotificationType.None)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name", "Name is null or empty.");
            if (string.IsNullOrEmpty(document)) throw new ArgumentNullException("document", "Document is null or empty.");
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email", "E-mail is null or empty.");

            var data = new CreateSignerRequest
            {
                Signer = new SignerRequest()
                {
                    Name = name,
                    Documentation = document,
                    Birthday = birthday.ToString("o"),
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Delivery = notify == NotificationType.Email ? "email" : (notify == NotificationType.SMS ? "sms" : "none"),
                    Auths = new string[] { "email" },
                    HasDocumentation = true
                }
            };
            log.Debug(string.Format("Create signer {0} with token {1}", email, token));

            var result = await SendAsync<CreateSignerResult>(HttpMethod.Post, "/api/v1/signers", data);
            if (result != null)
                return result.Signer;

            return null;
        }

        /// <summary>
        /// create signer to documento, more information visit <see cref="https://developers.clicksign.com/docs/adicionar-signatario-a-documento">Clicksign Rest API</see>
        /// </summary>
        public async Task<DocumentSignerResult> CreateDocumentSigner(string documentKey, string signerKey, SignerSign sign)
        {
            if (string.IsNullOrEmpty(documentKey)) throw new ArgumentNullException("documentKey", "Document key is null or empty.");
            if (string.IsNullOrEmpty(signerKey)) throw new ArgumentNullException("signerKey", "Signer Key is null or empty.");

            var data = new CreateDocumentSignerRequest
            {
                List = new DocumentSignerRequest()
                {
                    DocumentKey = documentKey,
                    SignerKey = signerKey,
                    Sign = sign.ToString().ToLower()
                }
            };
            log.Debug(string.Format("Create document signer {0} with token {1}", signerKey, token));

            var result = await SendAsync<CreateDocumentSignerResult>(HttpMethod.Post, "/api/v1/lists", data);
            if (result != null)
                return result.List;

            return null;
        }

        /// <summary>
        /// Rmove Document Signer, more information visit <see cref="https://developers.clicksign.com/docs/cancelar-documento">Clicksign Rest API</see>
        /// </summary>
        public Task RemoveDocumentSigner(string documentSignerKey)
        {
            if (string.IsNullOrEmpty(documentSignerKey)) throw new ArgumentNullException("documentSignerKey", "Document Signer Key is null or empty.");

            return SendAsync<dynamic>(HttpMethod.Delete, string.Format("/api/v1/lists/{0}", documentSignerKey));
        }

        /// <summary>
        /// Create Batch, more information visit <see cref="https://developers.clicksign.com/docs/criar-lote">Clicksign Rest API</see>
        /// </summary>              
        public async Task<BatchResult> CreateBatch(IList<string> documentKeys, string signerKey, bool summary = true)
        {
            if (documentKeys == null || documentKeys.Count() == 0) throw new ArgumentNullException("documentKeys", "Document keys is null or empty.");
            if (string.IsNullOrEmpty(signerKey)) throw new ArgumentNullException("signerKey", "Signer key is null or empty.");

            var data = new CreateBatchRequest
            {
                Batch = new BatchRequest()
                {
                    DocumentKeys = documentKeys.ToArray(),
                    SignerKey = signerKey,
                    Summary = summary
                }
            };
            var result = await SendAsync<CreateBatchResult>(HttpMethod.Post, "/api/v1/batches", data);
            if (result != null)
                return result.Batch;

            return null;
        }

        /// <summary>
        /// Notify email to signer, more information visit <see cref="https://developers.clicksign.com/docs/solicitar-assinatura-por-email">Clicksign Rest API</see>
        /// </summary>
        public Task NotifyEmail(string requestSignatureKey, string message, string url = null)
        {
            if (string.IsNullOrEmpty(requestSignatureKey)) throw new ArgumentNullException("requestSignatureKey", "Request Signature Key is null or empty.");
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException("message", "Message is null or empty.");

            var data = new NotifyEmailRequest
            {
                RequestSignatureKey = requestSignatureKey,
                Message = message,
                Url = url
            };
            log.Debug(string.Format("Notify email to signer {0} with Token {1}", requestSignatureKey, token));

            return SendAsync<dynamic>(HttpMethod.Post, "/api/v1/notifications", data);
        }

        /// <summary>
        /// Notify SMS to signer, more information visit <see cref="https://developers.clicksign.com/docs/solicitar-assinatura-por-sms">Clicksign Rest API</see>
        /// </summary>
        public Task NotifySMS(string requestSignatureKey)
        {
            if (string.IsNullOrEmpty(requestSignatureKey)) throw new ArgumentNullException("requestSignatureKey", "Request Signature Key is null or empty.");

            var data = new NotifySMSRequest
            {
                RequestSignatureKey = requestSignatureKey
            };
            log.Debug(string.Format("Notify SMS to signer {0} with Token {1}", requestSignatureKey, token));

            return SendAsync<dynamic>(HttpMethod.Post, "/api/v1/notify_by_sms", data);
        }
        
        #region [ Utils ]

        private async Task<T> SendAsync<T>(HttpMethod method, string path, object content = null)
        {
            string url = GetUrl(path);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(method, url))
            {
                try
                {
                    if (content != null)
                    {
                        var json = JsonConvert.SerializeObject(content, new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    using (var response = await client.SendAsync(request))
                    {
                        var stream = await response.Content.ReadAsStreamAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            return DeserializeJsonFromStream<T>(stream);
                        }
                        else
                        {
                            throw CreateException(stream, (int)response.StatusCode);
                        }
                    }
                }
                finally
                {
                    if (request.Content != null) request.Dispose();
                }
            }
        }

        private ClicksignException CreateException(Stream stream, int statusCode)
        {
            string message = "An error occurred while executing api.";
            IList<string> errors = null;

            var result = DeserializeJsonFromStream<ErrorResult>(stream);
            if (result != null)
            {
                errors = result.Errors;

                if (errors != null && errors.Count > 0)
                    message = string.Join(", ", result.Errors);
            }


            return new ClicksignException(message, statusCode, errors);
        }

        private T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var jr = new JsonSerializer();
                jr.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var searchResult = jr.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        public string GetUrl(string path)
        {
            string uri1 = Host.TrimEnd('/');
            path = path.TrimStart('/');

            var url = string.Format("{0}/{1}", uri1, path);
            url += url.IndexOf('?') >= 0 ? "&" : "?";
            url += "access_token=" + token;

            return url;

        }

        #endregion
    }

}
