using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SolaERP.Application
{
    public static class UseMediatR
    {
        public static void AddMediatR(this IServiceCollection collection)
        {
            collection.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
        }
    }
}
