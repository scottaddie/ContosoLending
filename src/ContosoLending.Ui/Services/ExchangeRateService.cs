using ContosoLending.Grpc;
using Grpc.Net.Client;
using static ContosoLending.Grpc.ExchangeRateManager;

namespace ContosoLending.Ui.Services
{
    public class ExchangeRateService
    {
        public double GetExchangeRate()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new ExchangeRateManagerClient(channel);
            var request = new ExchangeRateRequest
            {
                CurrencyTypeFrom = "USD",
                CurrencyTypeTo = "Lev",
            };

            ExchangeRateReply exchangeRate = client.GetExchangeRate(request);

            return exchangeRate.ExchangeRate;
        }
    }
}
