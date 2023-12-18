using Decor.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

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

        [Obsolete("Use '.AddScoped<T>().Decorated()'.")]
        public static IServiceCollection AddScopedDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
            => services.AddScoped(CreateDecorated<TImplementation>);

        [Obsolete("Use '.AddScoped<T1, T2>().Decorated()'.")]
        public static IServiceCollection AddScopedDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
            => services.AddScoped(CreateDecorated<TInterface, TImplementation>);

        [Obsolete("Use '.AddSingleton<T>().Decorated()'.")]
        public static IServiceCollection AddSingletonDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
            => services.AddSingleton(CreateDecorated<TImplementation>);

        [Obsolete("Use '.AddSingleton<T1, T2>().Decorated()'.")]
        public static IServiceCollection AddSingletonDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
            => services.AddSingleton(CreateDecorated<TInterface, TImplementation>);

        [Obsolete("Use '.AddTransient<T>().Decorated()'.")]
        public static IServiceCollection AddTransientDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
            => services.AddTransient(CreateDecorated<TImplementation>);

        [Obsolete("Use '.AddTransient<T1, T2>().Decorated()'.")]
        public static IServiceCollection AddTransientDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
            => services.AddTransient(CreateDecorated<TInterface, TImplementation>);


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
        public static IServiceCollection Decorated(this IServiceCollection services, Func<MethodInfo, Type[]> action = null)
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
            services.Insert(index, Decorate(descriptor, action));
            services.RemoveAt(index + 1);

            return services;
        }

        /// <summary>
        /// Decorates the specified service type descriptor inside <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">Service type to be decorated</typeparam>
        public static IServiceCollection Decorate<T>(this IServiceCollection services, Func<MethodInfo, Type[]> action = null) where T : class
        {
            services.AddDecor();

            var descriptors = services.Where(x => x.ServiceType == typeof(T)).ToArray();

            if (!descriptors.Any())
            {
                throw new ArgumentException($"Cannot find the service of type '{typeof(T)}' to decorate. " +
                    $"Add '{typeof(T)}' to service collection before trying to decorate it.");
            }

            foreach (var descriptor in descriptors)
            {
                var index = services.IndexOf(descriptor);
                services.Insert(index, Decorate(descriptor, action));
                services.RemoveAt(index + 1);
            }

            return services;
        }

        private static ServiceDescriptor Decorate(ServiceDescriptor serviceDescriptor, Func<MethodInfo, Type[]> action = null)
        {
            object DecoratedFactory(IServiceProvider serviceProvider)
            {
                var implementation = serviceProvider.GetInstance(serviceDescriptor);
                var decorator = serviceProvider.GetRequiredService<Decorator>();

                return decorator.For(serviceDescriptor.ServiceType, implementation, action);
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
