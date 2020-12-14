using Decor.UnitTests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Decor.UnitTests
{
    public class HttpClientTests
    {
        [Fact]
        public async Task I_can_decorate_typed_http_client_that_was_registered_with_interface_and_implementation_type()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpClient<ITypedHttpClient, TypedHttpClient>(client => client.BaseAddress = new Uri("https://www.microsoft.com/"));
            services
                .AddSingleton<TestDecorator>() // It's a singleton for testing purposes only, use transient in real code.
                .Decorate<ITypedHttpClient>();

            var serviceProvider = services.BuildServiceProvider();

            var sut = serviceProvider.GetRequiredService<ITypedHttpClient>();

            var decorator = serviceProvider.GetRequiredService<TestDecorator>();

            // Act
            var actual = await sut.Post();

            // Assert
            actual.StatusCode.Should().Be(HttpStatusCode.OK);
            decorator.WasInvoked.Should().BeTrue();
        }

        #region Setup
        public interface ITypedHttpClient
        {
            Task<HttpResponseMessage> Post();
        }

        public class TypedHttpClient : ITypedHttpClient
        {
            public TypedHttpClient(HttpClient httpClient)
            {
                HttpClient = httpClient;
            }

            public HttpClient HttpClient { get; }

            [Decorate(typeof(TestDecorator))]
            public async Task<HttpResponseMessage> Post() => await HttpClient.GetAsync("windows");
        }
        #endregion
    }
}
