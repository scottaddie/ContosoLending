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
}
