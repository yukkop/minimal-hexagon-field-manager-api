using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Hexagon.Configurations.Autofac;

public static class AutofacConfiguration
{
    public static void RegisterAutofac(this WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        // Register services directly with Autofac here. Don't
        // call builder.Populate(), that happens in AutofacServiceProviderFactory.
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            containerBuilder.RegisterModule(new AutofacModule()));
    }
}