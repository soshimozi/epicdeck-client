using System;
using System.Linq;
using EpiDeckClient.Services;
using System.Reflection;
using EpiDeckClient.Framework.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace EpiDeckClient.Framework.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddViewModels<TViewModelBase>(this IServiceCollection services)
        {
            var vmType = typeof(TViewModelBase);

            var viewModels =
                vmType.Assembly.ExportedTypes.Where(x => x.IsAssignableTo(vmType) && !x.IsAbstract && !x.IsInterface);

            foreach (var viewModel in viewModels)
            {
                services.AddSingleton(viewModel);
            }

            return services;
        }


        public static bool IsAssignableTo(this Type type, [NotNullWhen(true)] Type targetType)
        {
            return targetType?.IsAssignableFrom(type) ?? false;
        }

        public static IServiceCollection AddProducers(this IServiceCollection services)
        {
            var producerInterfaceType = typeof(Producer<>);
            var assembly = Assembly.GetExecutingAssembly(); // change this to whichever assembly you want to scan

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsGenericType &&
                    type.GetGenericTypeDefinition() == producerInterfaceType)
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }


    }
}