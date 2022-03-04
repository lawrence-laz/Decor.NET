using Decor.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

// TODO: WHat will happen to this package?

namespace Decor
{
    /// <summary>
    /// Provides extension methods for registering <see cref="Decorator"/> and decorated types.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Sets up a singleton <see cref="Decorator"/> provider to generate <see cref="IDecorator"/> instances.
        /// </summary>
        public static IServiceCollection AddDecor(this IServiceCollection services)
        {
            services.TryAddSingleton(x => new Decorator((type) => (IDecorator)x.GetRequiredService(type)));

            return services;
        }

        public static IServiceCollection TryAddScopedDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
        {
            services.TryAddScoped(CreateDecorated<TImplementation>);

            return services;
        }

        public static IServiceCollection TryAddScopedDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
        {
            services.TryAddScoped(CreateDecorated<TInterface, TImplementation>);

            return services;
        }

        public static IServiceCollection TryAddSingletonDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
        {
            services.TryAddSingleton(CreateDecorated<TImplementation>);

            return services;
        }

        public static IServiceCollection TryAddSingletonDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
        {
            services.TryAddSingleton(CreateDecorated<TInterface, TImplementation>);

            return services;
        }

        public static IServiceCollection TryAddTransientDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
        {
            services.TryAddTransient(CreateDecorated<TImplementation>);

            return services;
        }

        public static IServiceCollection TryAddTransientDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
        {
            services.TryAddTransient(CreateDecorated<TInterface, TImplementation>);

            return services;
        }

        /// <summary>
        /// Decorates the last registered descriptor inside <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection Decorated(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.Count == 0)
            {
                throw new ArgumentException("Cannot decorate an empty IServiceCollection.", nameof(services));
            }

            var descriptor = services.Last();
            var index = services.Count - 1;
            services.Insert(index, Decorate(descriptor));
            services.RemoveAt(index + 1);

            return services;
        }

        private static ServiceDescriptor Decorate(ServiceDescriptor serviceDescriptor)
        {
            object DecoratedFactory(IServiceProvider serviceProvider)
            {
                var implementation = serviceProvider.GetInstance(serviceDescriptor);
                var decorator = serviceProvider.GetRequiredService<Decorator>();

                return decorator.For(serviceDescriptor.ServiceType, implementation);
            }

            return ServiceDescriptor.Describe(
                serviceType: serviceDescriptor.ServiceType,
                implementationFactory: DecoratedFactory,
                lifetime: serviceDescriptor.Lifetime);
        }

        private static TImplementation CreateDecorated<TImplementation>(IServiceProvider serviceProvider)
            where TImplementation : class
        {
            var implementation = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
            var decorator = serviceProvider.GetRequiredService<Decorator>();

            return decorator.For(implementation);
        }

        private static TInterface CreateDecorated<TInterface, TImplementation>(IServiceProvider serviceProvider)
            where TImplementation : TInterface where TInterface : class
        {
            var implementation = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);
            var decorator = serviceProvider.GetRequiredService<Decorator>();

            return decorator.For<TInterface, TImplementation>(implementation);
        }
    }
}
