﻿using ContosoLending.DomainModel;
using ContosoLending.Grpc.Server;
using ContosoLending.Ui.Infrastructure;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

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
                CurrencyTypeFrom = GetCurrencyTypeAlias(currencyTypeFrom),
                CurrencyTypeTo = GetCurrencyTypeAlias(currencyTypeTo),
            };

            ExchangeRateReply exchangeRate = await client.GetExchangeRateAsync(request);

            return exchangeRate.ExchangeRate;
        }

        public async Task<decimal> GetConvertedAmountAsync(CurrencyConversion conversion)
        {
            decimal convertedAmount;
            decimal exchangeRate = Convert.ToDecimal(await GetExchangeRateAsync(
                conversion.CurrencyTypeFrom, conversion.CurrencyTypeTo));

            if (conversion.CurrencyTypeTo == Currency.BulgarianLev)
            {
                convertedAmount = decimal.Round(conversion.AmountToConvert / exchangeRate, 2);
            }
            else
            {
                convertedAmount = decimal.Round(conversion.AmountToConvert * exchangeRate, 2);
            }

            return convertedAmount;
        }

        public string GetCurrencyTypeAlias(Currency currencyType)
        {
            string currencyAlias = Constants.UsDollarAlias;
            
            if (currencyType == Currency.BulgarianLev)
            {
                currencyAlias = Constants.BulgarianLevAlias;
            }

            return currencyAlias;
        }

        public Currency GetCurrencyEnumValueFromSymbol(string currencyType)
        {
            Currency enumValue = Currency.USDollar;

            if (currencyType == Constants.BulgarianLevSymbol)
            {
                enumValue = Currency.BulgarianLev;
            }

            return enumValue;
        }
    }
}
