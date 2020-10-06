using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace APIGEECreateAPIKeyHelpers.Function
{
    public static class PrepJsonRequest_CreateAPIGEEAPIKey
    {
        [FunctionName("PrepJsonRequest_CreateAPIGEEAPIKey")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Received a request to compose json for creating new APIGEE API Credential");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                DeveloperApp developerApp = JsonConvert.DeserializeObject<DeveloperApp>(requestBody);
                DateTimeOffset expiryDateTimeOffset = DateTimeOffset.MinValue;
                AppApiKeyCrendential apiKeyCrendential = new AppApiKeyCrendential();
                if (developerApp != null)
                {
                    apiKeyCrendential.Name = developerApp.Name;
                    apiKeyCrendential.KeyExpiresIn = Convert.ToInt64(TimeSpan.FromHours(90 * 24).TotalMilliseconds);
                     apiKeyCrendential.ApiProducts = new List<string>();
                    foreach (var item in developerApp?.Credentials)
                    {
                        foreach (var product in item.ApiProducts)
                        {
                            apiKeyCrendential.ApiProducts.Add(product.Apiproduct);
                        }
                    }

                }
                return new OkObjectResult(JsonConvert.SerializeObject(apiKeyCrendential));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    public class Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ApiProduct
    {
        public string Apiproduct { get; set; }
        public string Status { get; set; }
    }

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

    public class AppApiKeyCrendential
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("apiProducts")]
        public List<string> ApiProducts { get; set; }

        [JsonProperty("keyExpiresIn")]
        public long KeyExpiresIn { get; set; }
    }

}
