

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Game 
    {
        public int maxRows { get; set; }
        public int maxCols { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get; set;
        }
        public List<Column> Grid
        {
            get; set;
        } = new List<Column>();

        public bool currentUser { get; set; }

        public int level { get; set; }
    }
}
