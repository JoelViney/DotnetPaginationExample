using System.Collections.Generic;

namespace CorePaginationExample
{
    /// <summary>
    /// This is used to return the results of a search as well as display:
    /// Page X of Y. Total results Z.
    /// </summary>
    public class Pagination<T>
    {
        /// <summary>The results of the search.</summary>
        public List<T> Items { get; internal set; }

        /// <summary>The currently selected page of results.</summary>
        public int CurrentPage { get; private set; }

        /// <summary>The total number of pages that can be viewed.</summary>
        public int TotalPages { get; private set; }

        /// <summary>The total number of results that the search criteria matched.</summary>
        public int TotalItems { get; private set; }

        /// <summary>Calculated by the pagination it tells the repositories how many pages to skip.</summary>
        internal int Skip { get; private set; }

        public int ItemsPerPage { get; private set; }

        public Pagination(int currentPage, int itemsPerPage, int totalItems)
        {
            if (itemsPerPage < 1)
            {
                itemsPerPage = 1;
            }
            this.ItemsPerPage = itemsPerPage;

            int totalPages;
            if (totalItems == 0)
            {
                totalPages = 1;
            }
            else if ((totalItems % itemsPerPage) == 0)
            {
                totalPages = (totalItems / itemsPerPage);
            }
            else
            {
                totalPages = (totalItems / itemsPerPage) + 1;
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            var skip = 0;
            if (currentPage > 1)
            {
                skip = (currentPage - 1) * itemsPerPage;
            }

            this.CurrentPage = currentPage;
            this.TotalPages = totalPages;
            this.TotalItems = totalItems;
            this.Skip = skip;
        }
    }
}
