using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ARchGLCloud.Application.MPP.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void AddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile),
                                   typeof(ViewModelToDomainMappingProfile));
        }
    }
}
