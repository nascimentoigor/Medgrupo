using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Teste.Api;
using Teste.Domain.Entities;
using Teste.Infrastructure.Data.Contexts;
using Xunit;

namespace Teste.Api.Integration.Tests
{
    public class CadastroTesteApiTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string DatabaseName = "Medgrupo";
        private const string ContatoGetEndpoint = "/Contato";
        private const string ContatoPostEndpoint = "/Contato";
        private const string PathJsonCorretoJson = "Json//InclusaoCorreta.json";
        private const string PathJsonErroJson = "Json//IncErro.json";

        private readonly WebApplicationFactory<Startup> _factory;

        public CadastroTesteApiTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ContatoGetEndpoint_Should_GetData()
        {
            // arrange
            var client = CreateClient();
            await using var context = GetDbContext();

            //act
            var response = await client.GetAsync(ContatoGetEndpoint);
            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<Contato>>(content);

            //assert
            Assert.Equal(2, list.Count());
            response.EnsureSuccessStatusCode();
        }
        

        [Fact]
        public async Task ContatoPostEndpoint_Should_Persist_Data_In_Database()
        {
            // arrange
            var client = CreateClient();

            //act
            var response = await client.PostAsync(ContatoPostEndpoint,
                new StringContent(await File.ReadAllTextAsync(PathJsonCorretoJson), Encoding.UTF8, "application/json"));

            //assert
            response.EnsureSuccessStatusCode();
            await using var context = GetDbContext();
            context.Contato.Count().Should().Be(3);
        }


        [Fact]
        public async Task ContatoPostEndpoint_Should_Persist_Data_In_Database_Whith_Error()
        {
            // arrange
            var client = CreateClient();

            //act
            var response = await client.PostAsync(ContatoPostEndpoint,
                new StringContent(await File.ReadAllTextAsync(PathJsonErroJson), Encoding.UTF8, "application/json"));

            //assert
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        }






        private static TesteContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase(DatabaseName);
            var context = new TesteContext(builder.Options);

            if (context.Contato.Count() == 0)
            {
                context.Contato.Add(new Domain.Entities.Contato() { Id = 1, Nome = "Igor", Ativo = true });
                context.Contato.Add(new Domain.Entities.Contato() { Id = 2, Nome = "Joao", Ativo = true });

                context.SaveChanges();
            }
            return context;
        }

        private HttpClient CreateClient() =>
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TesteContext>));
                    services.Remove(descriptor);
                    services.AddDbContext<TesteContext>(options => options.UseInMemoryDatabase(DatabaseName));

                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<TesteContext>();

                    db.Database.EnsureCreated();
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }
}