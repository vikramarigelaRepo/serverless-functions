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
using System.Linq;
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
                AppNewCrendential apiKeyCrendential = new AppNewCrendential();
                if (developerApp != null)
                {
                    apiKeyCrendential.Name = developerApp.Name;
                    apiKeyCrendential.KeyExpiresIn = Convert.ToInt64(TimeSpan.FromHours(90 * 24).TotalMilliseconds);
                    apiKeyCrendential.ApiProducts = new List<string>();
                    foreach (var item in developerApp.Credentials)
                    {
                        foreach (var product in item.ApiProducts)
                        {
                            if (!apiKeyCrendential.ApiProducts.Contains(product.Apiproduct))
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



}
