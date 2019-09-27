namespace ContosoLending.DomainModel
{
    public class LoanApplication
    {
        public Applicant Applicant { get; set; } = new Applicant();

        public double LoanAmount { get; set; }

        public string CurrencyType { get; set; }
    }
}
