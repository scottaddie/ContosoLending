using System;

namespace ContosoLending.DomainModel
{
    public class CurrencyConversion
    {
        public Currency CurrencyTypeFrom { get; set; }

        public Currency CurrencyTypeTo { get; set; }

        public decimal AmountToConvert { get; set; }
    }

    public enum Currency
    {
        USDollar = 0,
        BulgarianLev = 1,
    }

    public static class CurrencyExtensions
    {
        public static string ToAlias(this Currency currency) =>
            currency switch
            {
                Currency.BulgarianLev   => Constants.BulgarianLevAlias,
                Currency.USDollar       => Constants.UsDollarAlias,
                _                       => throw new ArgumentException(message: "invalid enum value", paramName: nameof(currency)),
            };
    }
}
