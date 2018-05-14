using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CorePaginationExample
{
    public class Widget
    {
        public Widget()
        {
            this.DateCreated = DateTime.UtcNow;
            this.Active = true;
        }

        [Column("WidgetID"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WidgetId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public bool Active { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsNew()
        {
            return this.WidgetId == 0;
        }
    }
}
