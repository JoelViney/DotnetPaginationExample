using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePaginationExample.Tests
{
    [TestClass]
    public class PaginationTests
    {
        /// <summary>Loads up the service using an in memory database.</summary>
        private DatabaseContext GetInMemoryContext()
        {
            // Create in-memory database.
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseInMemoryDatabase(String.Format("Test{0}", Guid.NewGuid()));

            var context = new DatabaseContext(optionsBuilder.Options);
            return context;
        }

        /// <summary>
        /// This tests that we get the first page when requesting it.
        /// </summary>
        [TestMethod]
        public async Task PaginateFirstPageTestAsync()
        {
            // Arrange
            var repository = new WidgetRepository(this.GetInMemoryContext());

            await repository.SaveAsync(new List<Widget>()
            {
                new Widget() { Name = "a" },
                new Widget() { Name = "b" },
                new Widget() { Name = "c" },

                new Widget() { Name = "d" },
            });

            // Act 
            const int Page = 1;
            const int ResultsPerPage = 3;
            List<Widget> list = await repository.SearchAsync(Page, ResultsPerPage);

            // Assert
            Assert.AreEqual(3, list.Count);
        }

        // This tests that we get the first page when requesting it
        [TestMethod]
        public async Task PaginateSecondPageTestAsync()
        {
            // Arrange
            var repository = new WidgetRepository(this.GetInMemoryContext());

            var widgets = new List<Widget>()
            {
                new Widget() { Name = "a" },
                new Widget() { Name = "b" },
                new Widget() { Name = "c" },

                new Widget() { Name = "d"},
                new Widget() { Name = "e" },
                new Widget() { Name = "f" },

                new Widget() { Name = "g" },
            };

            await repository.SaveAsync(widgets);

            // Act
            const int Page = 2;
            const int ResultsPerPage = 3;
            var list = await repository.SearchAsync(Page, ResultsPerPage);

            // Assert
            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(list.Any(x => x.Name == "d"));
            Assert.IsTrue(list.Any(x => x.Name == "e"));
            Assert.IsTrue(list.Any(x => x.Name == "f"));
        }


    }
}
