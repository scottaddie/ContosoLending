using System;
using System.ComponentModel.DataAnnotations;
using ContosoLending.Ui.Validation;

namespace ContosoLending.Ui.ViewModels
{
    public class LoanApplication
    {
        [ValidateRecursive]
        public Applicant Applicant { get; set; } = new Applicant();

        [Range(1, 100000)]
        public double LoanAmount { get; set; }

        public string CurrencyType { get; set; } = "$";
    }
}
