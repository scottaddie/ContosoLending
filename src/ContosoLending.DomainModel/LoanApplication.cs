﻿namespace ContosoLending.DomainModel
{
    public class LoanApplication
    {
        public Applicant Applicant { get; set; } = new Applicant();

        public LoanAmount LoanAmount { get; set; }
    }
}
