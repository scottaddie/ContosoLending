using System.ComponentModel.DataAnnotations;

namespace ContosoLending.Ui.ViewModels
{
    public class LoanApplication
    {
        [ValidateComplexType]
        public Applicant Applicant { get; set; } = new Applicant();

        [ValidateComplexType]
        public LoanAmount LoanAmount { get; set; } = new LoanAmount();
    }
}
