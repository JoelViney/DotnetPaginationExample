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
            var paginator = await this._repository.SearchAsync(1, 10, "toad");

            // Assert
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
            var paginator = await this._repository.SearchAsync(1, 10, "kanga");

            // Assert
            Assert.AreEqual("kangaroo", paginator.Items.First().Name);
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
            var paginator = await this._repository.SearchAsync(1, 10, null, true);

            // Assert
            Assert.AreEqual(2, paginator.Items.Count);
            Assert.IsFalse(paginator.Items.Any(x => x.Active == false));
        }
    }
}
