using DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestApiFactory>
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient Client;
    protected readonly DatabaseContext Db;

    protected BaseIntegrationTest(IntegrationTestApiFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Db = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        Db.Database.Migrate();
        Client = factory.CreateClient();
    }
}