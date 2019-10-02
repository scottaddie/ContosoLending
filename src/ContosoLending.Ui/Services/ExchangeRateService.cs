using ContosoLending.Grpc.Server;
using Grpc.Net.Client;
using static ContosoLending.Grpc.Server.ExchangeRateManager;

namespace ContosoLending.Ui.Services
{
    public class ExchangeRateService
    {
        public double GetExchangeRate(
            string currencyTypeFrom,
            string currencyTypeTo)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new ExchangeRateManagerClient(channel);
            var request = new ExchangeRateRequest
            {
                CurrencyTypeFrom = currencyTypeFrom,
                CurrencyTypeTo = currencyTypeTo,
            };

            ExchangeRateReply exchangeRate = client.GetExchangeRate(request);

            return exchangeRate.ExchangeRate;
        }
    }
}
