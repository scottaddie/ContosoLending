using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ContosoLending.Grpc.Server.Services
{
    public class ExchangeRateService : ExchangeRateManager.ExchangeRateManagerBase
    {
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(ILogger<ExchangeRateService> logger)
        {
            _logger = logger;
        }

        public override Task<ExchangeRateReply> GetExchangeRate(ExchangeRateRequest request, ServerCallContext context)
        {
            double exchangeRate = 0.00;

            if ((request.CurrencyTypeFrom == "USD" && request.CurrencyTypeTo == "Lev") ||
                (request.CurrencyTypeFrom == "Lev" && request.CurrencyTypeTo == "USD"))
            {
                exchangeRate = 0.56;
            }

            var reply = new ExchangeRateReply
            {
                ExchangeRate = exchangeRate
            };

            return Task.FromResult(reply);
        }
    }
}
