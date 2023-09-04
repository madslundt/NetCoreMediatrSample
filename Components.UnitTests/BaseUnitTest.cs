using DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Components.UnitTests;

public abstract class BaseUnitTest
{
    protected readonly DbContextOptions<DatabaseContext> DbContextOptions;

    public BaseUnitTest()
    {
        DbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
    }
}