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
            var newList = list.Where(x => x.IsNew());
            await this._context.AddRangeAsync(list);

            await this._context.SaveChangesAsync();
        }

        public async Task<List<Widget>> SearchAsync(int page, int resultsPerPage)
        {
            var query = (from x in this._context.Widgets
                         select x);

            var list = await query.Skip((page - 1) * resultsPerPage).Take(resultsPerPage).ToListAsync();

            return list;
        }
 
    }
}
