using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

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

        public static IServiceCollection AddScopedDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
            => services.AddScoped(CreateDecorated<TImplementation>);

        public static IServiceCollection AddScopedDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
            => services.AddScoped(CreateDecorated<TInterface, TImplementation>);

        public static IServiceCollection AddSingletonDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
            => services.AddSingleton(CreateDecorated<TImplementation>);

        public static IServiceCollection AddSingletonDecorated<TInterface, TImplementation>(
            this IServiceCollection services) where TImplementation : TInterface where TInterface : class
            => services.AddSingleton(CreateDecorated<TInterface, TImplementation>);

        public static IServiceCollection AddTransientDecorated<TImplementation>(
            this IServiceCollection services) where TImplementation : class
            => services.AddTransient(CreateDecorated<TImplementation>);

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
