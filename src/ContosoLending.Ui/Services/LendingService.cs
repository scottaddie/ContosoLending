using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using DM = ContosoLending.DomainModel;

namespace ContosoLending.Ui.Services
{
    public class LendingService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public LendingService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task SubmitLoanAppAsync(ViewModels.LoanApplication loanApp)
        {
            var model = _mapper.Map<DM.LoanApplication>(loanApp);
            await _httpClient.PostAsJsonAsync<DM.LoanApplication>(_httpClient.BaseAddress.AbsoluteUri, model);
        }
    }
}
