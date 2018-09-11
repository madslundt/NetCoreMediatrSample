using Microsoft.EntityFrameworkCore;
using System;

namespace Test.Common
{
    public static class DatabaseContextMock<T> where T : DbContext
    {
        public static DbContextOptions<T> InMemoryDatabase() 
        {
            DbContextOptions<T> options = new DbContextOptionsBuilder<T>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .Options;

            return options;
        }

        public static DbContextOptions<T> SQLite()
        {
            DbContextOptions<T> options = new DbContextOptionsBuilder<T>()
                  .UseSqlite("DataSource=:memory:")
                  .Options;

            return options;
        }
    }
}
