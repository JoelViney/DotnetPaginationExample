using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePaginationExample
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Widget> Widgets { get; set; }

        #region Constructors...

        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This will force the database context to use the in memory provider if no other is defined on startup.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }
    }
}
