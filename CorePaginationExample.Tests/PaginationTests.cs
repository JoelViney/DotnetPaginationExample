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
        private WidgetRepository _repository;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this._repository = new WidgetRepository(this.GetInMemoryContext());

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

            await this._repository.SaveAsync(widgets);
        }
        
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
            // Arrange - is done in the TestInitialize method.

            // Act 
            const int Page = 1;
            List<Widget> list = await this._repository.SearchAsync(Page, 3);

            // Assert
            Assert.AreEqual(3, list.Count);
        }


        /// <summary>
        /// This tests that we get the second page when requesting it
        /// </summary>
        [TestMethod]
        public async Task PaginateSecondPageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            const int Page = 2;
            var list = await this._repository.SearchAsync(Page, 3);

            // Assert
            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(list.Any(x => x.Name == "d"));
            Assert.IsTrue(list.Any(x => x.Name == "e"));
            Assert.IsTrue(list.Any(x => x.Name == "f"));
        }


        /// <summary>
        /// This tests that if we ask for a negative page number it just ignores it and retuns the first page.
        /// </summary>
        [TestMethod]
        public async Task PaginateNegativePageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            const int Page = -10;
            var list = await this._repository.SearchAsync(Page, 3);

            // Assert
            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(list.Any(x => x.Name == "a"));
            Assert.IsTrue(list.Any(x => x.Name == "b"));
            Assert.IsTrue(list.Any(x => x.Name == "c"));
        }


        /// <summary>
        /// This tests that if we ask for a crazy high page number it just returns the last one.
        /// </summary>
        [TestMethod]
        public async Task PaginatePage999of3TestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            const int Page = 999;
            var list = await this._repository.SearchAsync(Page, 3);

            // Assert
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.First().Name == "g");
        }

        /// <summary>
        /// This tests that if we ask for a crazy high page number it just returns the last one.
        /// </summary>
        [TestMethod]
        public async Task ResultsPerPageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            const int ResultsPerPage = 5;
            var list = await this._repository.SearchAsync(1, ResultsPerPage);

            // Assert
            Assert.AreEqual(5, list.Count);
        }

        /// <summary>
        /// This tests that if we ask for a crazy high page number it just returns the last one.
        /// </summary>
        [TestMethod]
        public async Task InvalidResultsPerPageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            const int ResultsPerPage = 0;
            var list = await this._repository.SearchAsync(1, ResultsPerPage);

            // Assert
            Assert.AreEqual(1, list.Count);
        }
    }
}
