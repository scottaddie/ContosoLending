using System.ComponentModel.DataAnnotations;

namespace ContosoLending.DomainModel
{
    public class LoanApplication
    {
        public Applicant Applicant { get; set; } = new Applicant();

        [Range(1, 100000)]
        public double LoanAmount { get; set; }
    }
}
