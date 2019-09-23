namespace ContosoLending.LoanProcessing.Models
{
    public class Applicant
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString() =>
            $"{LastName}, {FirstName}";
    }
}
