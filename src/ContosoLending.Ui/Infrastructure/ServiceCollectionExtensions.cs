using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoLending.Ui.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(config =>
            config.AddProfile(new MappingProfile()));

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ViewModels.Applicant, DomainModel.Applicant>();
            CreateMap<ViewModels.LoanAmount, DomainModel.LoanAmount>();
            CreateMap<ViewModels.LoanApplication, DomainModel.LoanApplication>();
        }
    }
}
