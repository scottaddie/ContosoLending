using System;
using System.Threading.Tasks;
using ContosoLending.CurrencyExchange;
using ContosoLending.DomainModel;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace ContosoLending.Ui.Services
{
    public class CurrencyConversionService
    {
        private readonly IConfiguration _configuration;

        public CurrencyConversionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private async Task<double> GetExchangeRateAsync(Currency currencyTypeFrom, Currency currencyTypeTo)
        {
            using var channel = GrpcChannel.ForAddress(_configuration["ExchangeRateService:BaseAddress"]);
            var client = new ExchangeRateManager.ExchangeRateManagerClient(channel);
            var request = new ExchangeRateRequest
            {
                CurrencyTypeFrom = currencyTypeFrom.ToAlias(),
                CurrencyTypeTo = currencyTypeTo.ToAlias(),
            };

            ExchangeRateReply exchangeRate = await client.GetExchangeRateAsync(request);

            return exchangeRate.ExchangeRate;
        }

        public async Task<decimal> GetConvertedAmountAsync(CurrencyConversion conversion)
        {
            decimal exchangeRate = Convert.ToDecimal(await GetExchangeRateAsync(
                conversion.CurrencyTypeFrom, conversion.CurrencyTypeTo));

            decimal convertedAmount = conversion.CurrencyTypeTo switch
            {
                Currency.BulgarianLev => decimal.Round(conversion.AmountToConvert / exchangeRate, 2),
                _ => decimal.Round(conversion.AmountToConvert * exchangeRate, 2),
            };

            return convertedAmount;
        }

        public Currency GetCurrencyEnumValueFromSymbol(string currencyType) =>
            currencyType switch
            {
                Constants.BulgarianLevSymbol    => Currency.BulgarianLev,
                Constants.UsDollarSymbol        => Currency.USDollar,
                _                               => throw new ArgumentException(message: "invalid currency type", paramName: nameof(currencyType)),
            };
    }
}
