using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContosoLending.Ui.ViewModels
{
    public class LoanAmount
    {
        [DisplayName("Loan Amount")]
        [Range(1, 100000)]
        public decimal Amount { get; set; } = 0.00m;

        public string CurrencyType { get; set; } = "$";
    }
}
