﻿using Microsoft.EntityFrameworkCore;

namespace CorePaginationExample
{
    /// <summary>Defines the order of the search.</summary>
    public enum WidgetOrderBy
    {
        Name,
        DateCreated
    }

    public class WidgetRepository
    {
        private readonly DatabaseContext _context;

        public WidgetRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(IEnumerable<Widget> list)
        {
            // This is a bit basic but it's all we need it to do for the demo.
            var newList = list.Where(x => x.IsNew());
            await _context.AddRangeAsync(list);

            await _context.SaveChangesAsync();
        }

        // Note: I would usually pass in a search object as the criteria can get quite complex.
        public async Task<Pagination<Widget>> SearchAsync(int page, int itemsPerPage, string criteria = null, bool activeOnly = false, WidgetOrderBy orderBy = WidgetOrderBy.Name)
        {
            var query = (from x in _context.Widgets
                         where
                         (
                            (String.IsNullOrEmpty(criteria) || x.Name.Contains(criteria, StringComparison.CurrentCultureIgnoreCase))
                             && (activeOnly == false || x.Active == true)
                         )
                         select x);

            var count = await query.CountAsync();

            var pagination = new Pagination<Widget>(page, itemsPerPage, count);

            IOrderedQueryable<Widget> orderedQuery;
            switch (orderBy)
            {
                case WidgetOrderBy.DateCreated: orderedQuery = query.OrderBy(o => o.DateCreated); break;
                case WidgetOrderBy.Name: orderedQuery = query.OrderBy(o => o.Name); break;
                default: orderedQuery = query.OrderBy(o => o.Name); break;
            }

            pagination.Items = await orderedQuery.Skip(pagination.Skip).Take(pagination.ItemsPerPage).ToListAsync();

            return pagination;
        }

    }
}
