using System;
using System.Threading.Tasks;
using ContosoLending.LoanProcessing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace ContosoLending.LoanProcessing.Functions
{
    public static partial class Functions
    {
        [FunctionName(nameof(CheckCreditAgency))]
        public async static Task<CreditAgencyResult> CheckCreditAgency(
            [ActivityTrigger] CreditAgencyRequest request,
            [SignalR(HubName = "dashboard")] IAsyncCollector<SignalRMessage> dashboardMessages,
            ILogger log)
        {
            log.LogWarning($"Checking agency {request.AgencyName} for customer {request.Application.Applicant.ToString()} for {request.Application.LoanAmount}");

            await dashboardMessages.AddAsync(new SignalRMessage
            {
                Target = "agencyCheckStarted",
                Arguments = new object[] { request }
            });

            var rnd = new Random();
            await Task.Delay(rnd.Next(2000, 4000)); // simulate variant processing times

            var result = new CreditAgencyResult
            {
                IsApproved = !(request.AgencyName.Contains("Woodgrove") && request.Application.LoanAmount.Amount > 4999),
                Application = request.Application,
                AgencyId = request.AgencyId
            };

            await dashboardMessages.AddAsync(new SignalRMessage
            {
                Target = "agencyCheckComplete",
                Arguments = new object[] { result }
            });

            log.LogWarning($"Agency {request.AgencyName} {(result.IsApproved ? "APPROVED" : "DECLINED")} request by customer {request.Application.Applicant.ToString()} for {request.Application.LoanAmount}");

            return result;
        }
    }
}
