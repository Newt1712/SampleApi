using Microsoft.Extensions.DependencyInjection;
using Web.Application.AutoMapper;
using Web.Application.Services;

namespace Web.Application.DI
{
    public static class InitService
    {
        #region Add Mapper
        public static IServiceCollection InitAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
        #endregion

        #region Add Serivces
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClubService, ClubService>();
            return services;
        }
        #endregion    
    }
}
