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
    public class OrderByTests
    {
        private WidgetRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            this._repository = new WidgetRepository(DatabaseHelper.GetInMemoryContext());
        }


        /// <summary>
        /// This tests to see that the correct ordering is applied.
        /// </summary>
        [TestMethod]
        public async Task OrderByDefaultTestAsync()
        {
            // Arrange
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "a" },
                new Widget() { Name = "c" },
                new Widget() { Name = "b" },
            };
            await this._repository.SaveAsync(widgets);

            // Act
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 10);

            // Assert
            Assert.AreEqual(3, paginator.Items.Count);
            Assert.AreEqual("a", paginator.Items[0].Name);
            Assert.AreEqual("b", paginator.Items[1].Name);
            Assert.AreEqual("c", paginator.Items[2].Name);
        }


        /// <summary>
        /// This tests to see that the non default ordering is applied.
        /// </summary>
        [TestMethod]
        public async Task OrderByAlternateTestAsync()
        {
            // Arrange
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "a", DateCreated = new DateTime(2000, 01, 01) },
                new Widget() { Name = "b", DateCreated = new DateTime(2000, 01, 03) },
                new Widget() { Name = "c", DateCreated = new DateTime(2000, 01, 02) },
            };
            await this._repository.SaveAsync(widgets);

            // Act
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 10, orderBy: WidgetOrderBy.DateCreated);

            // Assert
            Assert.AreEqual(3, paginator.Items.Count);
            Assert.AreEqual("a", paginator.Items[0].Name);
            Assert.AreEqual("c", paginator.Items[1].Name);
            Assert.AreEqual("b", paginator.Items[2].Name);
        }
    }
}
