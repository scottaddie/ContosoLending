using ContosoLending.DomainModel;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
            using var jsonStream = await req.Content.ReadAsStreamAsync();
            var loanApplication = await JsonSerializer.DeserializeAsync<LoanApplication>(jsonStream);

            string instanceId = await starter.StartNewAsync(nameof(Orchestrate), loanApplication);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
