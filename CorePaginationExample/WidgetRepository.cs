using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePaginationExample
{
    public class WidgetRepository
    {
        private DatabaseContext _context;

        public WidgetRepository(DatabaseContext context)
        {
            this._context = context;
        }

        public async Task SaveAsync(IEnumerable<Widget> list)
        {
            // This is a bit basic but it's all we need it to do.
            var newList = list.Where(x => x.IsNew());
            await this._context.AddRangeAsync(list);

            await this._context.SaveChangesAsync();
        }

        public async Task<List<Widget>> SearchAsync(int page, int resultsPerPage, string criteria = null, bool activeOnly = false)
        {
            if (resultsPerPage < 1)
                resultsPerPage = 1;

            var query = (from x in this._context.Widgets
                         where
                         (
                            (String.IsNullOrEmpty(criteria) || x.Name.ToLower().Contains(criteria.ToLower()))
                             && (activeOnly == false || x.Active == true)
                         )
                         select x);

            var count = await query.CountAsync();

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

            var list = await query.Skip(skip).Take(resultsPerPage).ToListAsync();

            return list;
        }
 
    }
}
