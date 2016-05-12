using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Column
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ColumnNumber { get; set; }
        public List<Row> theRows
        {
            get; set;
        } = new List<Row>();

        //public virtual ICollection<Row> theRows { get; set; } 

        int gameID { get; set; }
    }
}
