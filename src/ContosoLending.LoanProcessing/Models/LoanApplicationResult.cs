using ContosoLending.DomainModel;

namespace ContosoLending.LoanProcessing.Models
{
    public class LoanApplicationResult
    {
        public LoanApplication Application { get; set; }
        public bool IsApproved { get; set; }
    }
}
