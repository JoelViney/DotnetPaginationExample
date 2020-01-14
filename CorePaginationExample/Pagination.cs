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
        public int PageNumber { get; private set; }

        /// <summary>The total number of pages that can be viewed.</summary>
        public int TotalPages { get; private set; }

        /// <summary>The total number of results that the search criteria matched.</summary>
        public int RecordCount { get; private set; }

        /// <summary>Calculated by the pagination it tells the repositories how many pages to skip.</summary>
        internal int Skip { get; private set; }

        public int ResultsPerPage { get; private set; }

        public Pagination(int page, int resultsPerPage, int count)
        {
            // This is calculated here so we wont have to calculate it for each Repository
            if (resultsPerPage < 1)
                resultsPerPage = 1;
            this.ResultsPerPage = resultsPerPage;

            var totalPages = 1;
            if (count == 0)
                totalPages = 1;
            else if ((count % resultsPerPage) == 0)
                totalPages = (count / resultsPerPage);
            else
                totalPages = (count / resultsPerPage) + 1;

            if (page > totalPages)
                page = totalPages;

            var skip = 0;
            if (page > 1)
            {
                skip = (page - 1) * resultsPerPage;
            }

            this.PageNumber = page;
            this.TotalPages = totalPages;
            this.RecordCount = count;
            this.Skip = skip;
        }
    }
}
