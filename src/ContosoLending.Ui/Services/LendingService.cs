using System.Net.Http;
using System.Threading.Tasks;
using ContosoLending.DomainModel;
using Microsoft.AspNetCore.Components;

namespace ContosoLending.Ui.Services
{
    public class LendingService
    {
        private readonly string _route;
        private readonly HttpClient _httpClient;

        public LendingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _route = httpClient.BaseAddress.AbsoluteUri;
        }

        public async Task SubmitLoanAppAsync(LoanApplication loanApp)
        {
            await _httpClient.PostJsonAsync<LoanApplication>(_route, loanApp);
        }
    }
}
