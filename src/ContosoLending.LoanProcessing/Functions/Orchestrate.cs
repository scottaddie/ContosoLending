using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoLending.DomainModel;
using ContosoLending.LoanProcessing.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace ContosoLending.LoanProcessing.Functions
{
    public static partial class Functions
    {
        [FunctionName(nameof(Orchestrate))]
        public static async Task<LoanApplicationResult> Orchestrate(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            [SignalR(HubName = "dashboard")] IAsyncCollector<SignalRMessage> dashboardMessages,
            ILogger logger)
        {
            var loanApplication = context.GetInput<LoanApplication>();
            var agencyTasks = new List<Task<CreditAgencyResult>>();
            var agencies = new List<CreditAgencyRequest>();
            var results = new CreditAgencyResult[] { };

            logger.LogWarning($"Status of application for {loanApplication.Applicant.ToString()} for {loanApplication.LoanAmount}: Checking with agencies.");

            // start the process and perform initial validation
            bool loanStarted = await context.CallActivityAsync<bool>(nameof(Receive), loanApplication);

            // fan out and check the credit agencies
            if (loanStarted)
            {
                agencies.AddRange(new CreditAgencyRequest[] {
                    new CreditAgencyRequest { AgencyName = "Contoso, Ltd.", AgencyId = "contoso", Application = loanApplication },
                    new CreditAgencyRequest { AgencyName = "Fabrikam, Inc.", AgencyId = "fabrikam", Application = loanApplication },
                    new CreditAgencyRequest { AgencyName = "Woodgrove Bank", AgencyId = "woodgrove", Application = loanApplication },
                });

                foreach (var agency in agencies)
                {
                    agencyTasks.Add(context.CallActivityAsync<CreditAgencyResult>(nameof(CheckCreditAgency), agency));
                }

                await dashboardMessages.AddAsync(new SignalRMessage
                {
                    Target = "agencyCheckPhaseStarted",
                    Arguments = new object[] { }
                });

                // wait for all the agencies to return their results
                results = await Task.WhenAll(agencyTasks);

                await dashboardMessages.AddAsync(new SignalRMessage
                {
                    Target = "agencyCheckPhaseCompleted",
                    Arguments = new object[] { !(results.Any(x => x.IsApproved == false)) }
                });
            }

            var response = new LoanApplicationResult
            {
                Application = loanApplication,
                IsApproved = loanStarted && !(results.Any(x => x.IsApproved == false))
            };

            logger.LogWarning($"Agency checks result with {response.IsApproved} for loan amount of {response.Application.LoanAmount} to customer {response.Application.Applicant.ToString()}");

            await dashboardMessages.AddAsync(new SignalRMessage
            {
                Target = "loanApplicationComplete",
                Arguments = new object[] { response }
            });

            return response;
        }
    }
}
