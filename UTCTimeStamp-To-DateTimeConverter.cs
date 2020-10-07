using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace DateTimeConverterHelpers.Function
{
    public static class UTCTimeStamp_To_DateTimeConverter
    {
        [FunctionName("UTCTimeStamp_To_DateTimeConverter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Received a request to get api key credentials.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DeveloperApp apigeeDeveloperApp = JsonConvert.DeserializeObject<DeveloperApp>(requestBody);
            AppRecentCredential appRecentCredential = null;
            if (apigeeDeveloperApp != null)
            {
                var credential = apigeeDeveloperApp.Credentials.OrderByDescending(p => DateTimeOffset.FromUnixTimeMilliseconds(p.IssuedAt)).First();
                if (credential != null)
                {
                    appRecentCredential = new AppRecentCredential
                    {
                        AppName = apigeeDeveloperApp.Name,
                        APIKey = credential.ConsumerKey,
                        DaysToExpiry = (DateTimeOffset.FromUnixTimeMilliseconds(credential.ExpiresAt).UtcDateTime - DateTime.UtcNow).Days,
                        ExpirationDate  = DateTimeOffset.FromUnixTimeMilliseconds(credential.ExpiresAt).UtcDateTime.ToShortDateString()
                    };
                }
            }
            string responseMessage = $" APIGEE API key {appRecentCredential.APIKey} is expiring on {appRecentCredential.ExpirationDate}";
            return new OkObjectResult(appRecentCredential);

        }
    }
}
