using CorePaginationExample.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePaginationExample
{
    [TestClass]
    public class CriteriaTests
    {
        private WidgetRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            this._repository = new WidgetRepository(DatabaseHelper.GetInMemoryContext());
        }


        /// <summary>
        /// This tests that we get the first page when requesting it.
        /// </summary>
        [TestMethod]
        public async Task CriteriaFullMatchTestAsync()
        {
            // Arrange
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "frog" },
                new Widget() { Name = "toad" },
                new Widget() { Name = "kangaroo" },
            };
            await this._repository.SaveAsync(widgets);

            // Act 
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 10, criteria: "toad");

            // Assert
            Assert.AreEqual(1, paginator.Items.Count);
            Assert.AreEqual("toad", paginator.Items.First().Name);
        }


        /// <summary>
        /// This tests that we get the first page when requesting it.
        /// </summary>
        [TestMethod]
        public async Task CriteriaPartialMatchTestAsync()
        {
            // Arrange 
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "frog" },
                new Widget() { Name = "toad" },
                new Widget() { Name = "kangaroo" },
            };
            await this._repository.SaveAsync(widgets);

            // Act 
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 10, criteria: "kanga");

            // Assert
            Assert.AreEqual(1, paginator.Items.Count);
            Assert.AreEqual("kangaroo", paginator.Items.First().Name);
        }


        /// <summary>
        /// This tests that we get the first page when requesting it.
        /// </summary>
        [TestMethod]
        public async Task CriteriaMatchMultipleTestAsync()
        {
            // Arrange 
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "frog" },
                new Widget() { Name = "toad" },
                new Widget() { Name = "big kangaroo" },
                new Widget() { Name = "small kangaroo" },
            };
            await this._repository.SaveAsync(widgets);

            // Act 
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 10, criteria: "kanga");

            // Assert
            Assert.AreEqual(2, paginator.Items.Count);
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "big kangaroo"));
            Assert.IsTrue(paginator.Items.Any(x => x.Name == "small kangaroo"));
        }


        // This tests to see that the active only filter is applied.
        [TestMethod]
        public async Task ActiveOnlySearchTestAsync()
        {
            // Arrange
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "a", Active = true },
                new Widget() { Name = "b", Active = false },
                new Widget() { Name = "c", Active = true },
            };
            await this._repository.SaveAsync(widgets);

            // Act
            var paginator = await this._repository.SearchAsync(page: 1, resultsPerPage: 10, criteria: null, activeOnly: true);

            // Assert
            Assert.AreEqual(2, paginator.Items.Count);
            Assert.IsFalse(paginator.Items.Any(x => x.Active == false));
        }
    }
}
