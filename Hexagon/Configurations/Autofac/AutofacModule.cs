using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Hexagon.Configurations.Autofac;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = Assembly.Load(new AssemblyName("Hexagon.Database"));
        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("Repository"));

        assembly = Assembly.Load(new AssemblyName("Hexagon.Logic"));
        builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("Logic"))
            .AsImplementedInterfaces();
    }
}