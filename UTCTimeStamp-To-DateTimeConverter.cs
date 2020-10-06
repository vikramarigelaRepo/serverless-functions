using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DateTimeConverterHelpers.Function
{
    public static class UTCTimeStamp_To_DateTimeConverter
    {
        [FunctionName("UTCTimeStamp_To_DateTimeConverter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Received a request to convert APIGEE API Expiration Time.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string apiKey = null;
            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            DateTimeOffset expiryDateTimeOffset = DateTimeOffset.MinValue;
            if (data != null)
            {
                foreach (var item in data?.credentials)
                {
                    string keyExpiry = item?.expiresAt;
                    apiKey = item?.consumerKey;
                    expiryDateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(keyExpiry));
                    // dtDateTime = dtDateTime.(double.Parse(keyExpiry.Substring(0, keyExpiry.Length - 3))).ToLocalTime();
                }
            }

            string responseMessage = $" APIGEE API key {apiKey} is expiring on {expiryDateTimeOffset.Date}";
            return new OkObjectResult((expiryDateTimeOffset.DateTime - DateTime.UtcNow).Days);

        }
    }
}
