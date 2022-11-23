using AutoMapper;
using Hexagon.Logic;

namespace Hexagon.Configurations;

public static class AutoMapperConfiguration
{
    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
    }
}