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


        // This tests to see that the correct ordering is applied.
        [TestMethod]
        public async Task DefaultOrderByTestAsync()
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
            var list = await this._repository.SearchAsync(1, 10);

            // Assert
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("a", list[0].Name);
            Assert.AreEqual("b", list[1].Name);
            Assert.AreEqual("c", list[2].Name);
        }

        // This tests to see that the non default ordering is applied.
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
            var list = await this._repository.SearchAsync(1, 10, orderBy: WidgetOrderBy.DateCreated);

            // Assert
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("a", list[0].Name);
            Assert.AreEqual("c", list[1].Name);
            Assert.AreEqual("b", list[2].Name);
        }
    }
}
