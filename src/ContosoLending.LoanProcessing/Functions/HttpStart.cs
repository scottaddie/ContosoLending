using System.Net.Http;
using System.Threading.Tasks;
using ContosoLending.LoanProcessing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ContosoLending.LoanProcessing.Functions
{
    public static partial class Functions
    {
        [FunctionName(nameof(HttpStart))]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            var json = await req.Content.ReadAsStringAsync();
            // TODO: migrate to System.Text.Json
            var loanApplication = JsonConvert.DeserializeObject<LoanApplication>(json);

            string instanceId = await starter.StartNewAsync(nameof(Orchestrate), loanApplication);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
