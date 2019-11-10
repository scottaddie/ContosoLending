using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static ContosoLending.DomainModel.Constants;

namespace ContosoLending.CurrencyExchange.Services
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

            if ((request.CurrencyTypeFrom == UsDollarAlias && request.CurrencyTypeTo == BulgarianLevAlias) ||
                (request.CurrencyTypeFrom == BulgarianLevAlias && request.CurrencyTypeTo == UsDollarAlias))
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
