using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace ContosoLending.LoanProcessing.Functions
{
    public static partial class Functions
    {
        [FunctionName(nameof(Negotiate))]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "dashboard")] SignalRConnectionInfo connectionInfo) => connectionInfo;
    }
}
