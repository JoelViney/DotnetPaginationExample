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
    public class PaginatorReportingTests
    {
        private WidgetRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            this._repository = new WidgetRepository(DatabaseHelper.GetInMemoryContext());
        }


        /// <summary>
        /// Tests that in ther user interface we can output information such as: 
        /// Page X of Y. Total results Z.
        /// </summary>
        [TestMethod]
        public async Task PaginatorReportingSunnyDayTestAsync()
        {
            // Arrange
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "a" },
                new Widget() { Name = "b" },
                new Widget() { Name = "c" },

                new Widget() { Name = "d" },
                new Widget() { Name = "e" },
                new Widget() { Name = "f" },

                new Widget() { Name = "g" },
                new Widget() { Name = "h" },
            };
            await this._repository.SaveAsync(widgets);

            // Act
            var paginator = await this._repository.SearchAsync(page: 2, resultsPerPage: 3);

            // Assert
            Assert.AreEqual(2, paginator.PageNumber);
            Assert.AreEqual(3, paginator.TotalPages);
            Assert.AreEqual(8, paginator.RecordCount);
        }
    }
}
