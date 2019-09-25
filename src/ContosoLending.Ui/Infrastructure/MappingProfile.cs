﻿using AutoMapper;

namespace ContosoLending.Ui.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ViewModels.Applicant, DomainModel.Applicant>();
            CreateMap<ViewModels.LoanApplication, DomainModel.LoanApplication>();
        }
    }
}