using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Views
{
    public static class QuantityViewFunction
    {
        [FunctionName("QuantityView")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("starting function, yeah...");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            log.LogInformation($"json body: {requestBody}");

            dynamic data = JsonConvert.DeserializeObject(requestBody);
            int donationId = data?.Id;

            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var textSql = $@"UPDATE [dbo].[Donation] SET [QuantityView] = [QuantityView] + 1 WHERE [Id] = {donationId};";

                using (SqlCommand cmd = new SqlCommand(textSql, conn))
                {
                    var rowsAffected = cmd.ExecuteNonQuery();
                    log.LogInformation($"rowsAffected: {rowsAffected}");
                }
            }

            return new OkResult();
        }
    }
}
