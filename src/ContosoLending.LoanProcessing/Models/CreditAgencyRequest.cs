using ContosoLending.DomainModel;

namespace ContosoLending.LoanProcessing.Models
{
    public class CreditAgencyRequest
    {
        public string AgencyId { get; set; }
        public string AgencyName { get; set; }
        public LoanApplication Application { get; set; }
    }
}
