namespace CorePaginationExample
{
    [TestClass]
    public class CriteriaTests
    {
        private WidgetRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new WidgetRepository(DatabaseHelper.GetInMemoryContext());
        }

        /// <summary>
        /// Tests that search criteria works.
        /// </summary>
        [TestMethod]
        public async Task CriteriaFullMatch()
        {
            // Arrange
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "frog" },
                new Widget() { Name = "toad" },
                new Widget() { Name = "kangaroo" },
            };
            await _repository.SaveAsync(widgets);

            // Act 
            var pagination = await _repository.SearchAsync(page: 1, itemsPerPage: 10, criteria: "toad");

            // Assert
            Assert.AreEqual(1, pagination.Items.Count);
            Assert.AreEqual("toad", pagination.Items.First().Name);
        }


        /// <summary>
        /// Tests that partial matches work.
        /// </summary>
        [TestMethod]
        public async Task CriteriaPartialMatch()
        {
            // Arrange 
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "frog" },
                new Widget() { Name = "toad" },
                new Widget() { Name = "kangaroo" },
            };
            await _repository.SaveAsync(widgets);

            // Act 
            var pagination = await _repository.SearchAsync(page: 1, itemsPerPage: 10, criteria: "kanga");

            // Assert
            Assert.AreEqual(1, pagination.Items.Count);
            Assert.AreEqual("kangaroo", pagination.Items.First().Name);
        }


        /// <summary>
        /// Tests that partial matches can match multiple records.
        /// </summary>
        [TestMethod]
        public async Task CriteriaMultipleMatch()
        {
            // Arrange 
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "frog" },
                new Widget() { Name = "toad" },
                new Widget() { Name = "big kangaroo" },
                new Widget() { Name = "small kangaroo" },
            };
            await _repository.SaveAsync(widgets);

            // Act 
            var pagination = await _repository.SearchAsync(page: 1, itemsPerPage: 10, criteria: "kanga");

            // Assert
            Assert.AreEqual(2, pagination.Items.Count);
            Assert.IsTrue(pagination.Items.Any(x => x.Name == "big kangaroo"));
            Assert.IsTrue(pagination.Items.Any(x => x.Name == "small kangaroo"));
        }


        /// <summary>
        /// Tests that boolean criteria works.
        /// </summary>
        [TestMethod]
        public async Task CriteriaActiveOnly()
        {
            // Arrange
            var widgets = new List<Widget>()
            {
                new Widget() { Name = "a", Active = true },
                new Widget() { Name = "b", Active = false },
                new Widget() { Name = "c", Active = true },
            };
            await _repository.SaveAsync(widgets);

            // Act
            var pagination = await _repository.SearchAsync(page: 1, itemsPerPage: 10, criteria: null, activeOnly: true);

            // Assert
            Assert.AreEqual(2, pagination.Items.Count);
            Assert.IsFalse(pagination.Items.Any(x => x.Active == false));
        }
    }
}
