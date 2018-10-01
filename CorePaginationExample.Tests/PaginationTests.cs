using CorePaginationExample.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePaginationExample
{
    [TestClass]
    public class PaginationTests
    {
        private WidgetRepository _repository;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this._repository = new WidgetRepository(DatabaseHelper.GetInMemoryContext());

            // Generates the test data for all the pagination tests.
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


        /// <summary>
        /// This tests that the resultsPerPage returns the correct number of items.
        /// </summary>
        [TestMethod]
        public async Task ResultsPerPageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 5);

            // Assert
            Assert.AreEqual(5, paginator.Items.Count);
        }


        /// <summary>
        /// This tests that we get the first page when requesting it.
        /// </summary>
        [TestMethod]
        public async Task PaginateFirstPageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act 
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 3);

            // Assert
            Assert.AreEqual(3, paginator.Items.Count);
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "a"));
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "b"));
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "c"));
        }


        /// <summary>
        /// This tests that we get the second page when requesting it
        /// </summary>
        [TestMethod]
        public async Task PaginateSecondPageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            var paginator = await this._repository.SearchAsync(page: 2, resultsPerPage: 3);

            // Assert
            Assert.AreEqual(3, paginator.Items.Count);
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "d"));
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "e"));
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "f"));
        }


        /// <summary>
        /// This tests that if we ask for a negative page number it just ignores it and retuns the first page.
        /// </summary>
        [TestMethod]
        public async Task PaginateNegativePageTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            var paginator = await this._repository.SearchAsync(page: -10, resultsPerPage: 3);

            // Assert
            Assert.AreEqual(3, paginator.Items.Count);
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "a"));
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "b"));
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "c"));
        }


        /// <summary>
        /// This tests that if we ask for a crazy high page number it just returns the last one.
        /// It also tests that if the last page doesn't contain a resultsPerPage of items it
        /// only returns whats left.
        /// </summary>
        [TestMethod]
        public async Task PaginatePage999of3TestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            var paginator = await this._repository.SearchAsync(page: 999, resultsPerPage: 3);

            // Assert
            Assert.AreEqual(1, paginator.Items.Count);
            Assert.IsTrue(paginator.Items.First().Name == "g");
        }


        /// <summary>
        /// This tests that if we ask for a crazy high page number it just returns the last one.
        /// </summary>
        [TestMethod]
        public async Task ResultsPerPageInvalidTestAsync()
        {
            // Arrange - is done in the TestInitialize method.

            // Act
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 0);

            // Assert
            Assert.AreEqual(1, paginator.Items.Count);
        }
    }
}
