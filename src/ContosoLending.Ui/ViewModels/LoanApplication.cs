using ContosoLending.Ui.Validation;

namespace ContosoLending.Ui.ViewModels
{
    public class LoanApplication
    {
        [ValidateRecursive]
        public Applicant Applicant { get; set; } = new Applicant();

        [ValidateRecursive]
        public LoanAmount LoanAmount { get; set; } = new LoanAmount();
    }
}
