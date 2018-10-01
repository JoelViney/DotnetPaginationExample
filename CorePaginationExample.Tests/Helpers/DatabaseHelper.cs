using Microsoft.EntityFrameworkCore;
using System;

namespace CorePaginationExample.Helpers
{
    internal static class DatabaseHelper
    {
        /// <summary>Creates an in memory database context.</summary>
        internal static DatabaseContext GetInMemoryContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseInMemoryDatabase(String.Format("Test{0}", Guid.NewGuid()));

            var context = new DatabaseContext(optionsBuilder.Options);
            return context;
        }
    }
}
