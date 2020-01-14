using CorePaginationExample.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CorePaginationExample
{
    [TestClass]
    public class PaginationReportingTests
    {
        private WidgetRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new WidgetRepository(DatabaseHelper.GetInMemoryContext());
        }


        /// <summary>
        /// Tests that in ther user interface we can output information such as: 
        /// Page X of Y. Total results Z.
        /// </summary>
        [TestMethod]
        public async Task PaginationReportingSunnyDayTestAsync()
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
            await _repository.SaveAsync(widgets);

            // Act
            var pagination = await _repository.SearchAsync(page: 2, resultsPerPage: 3);

            // Assert
            Assert.AreEqual(2, pagination.PageNumber);
            Assert.AreEqual(3, pagination.TotalPages);
            Assert.AreEqual(8, pagination.RecordCount);
        }
    }
}
