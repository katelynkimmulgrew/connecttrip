using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class Ques
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string description { get; set; }

        public virtual ICollection<theAnswer> answers2 { get; set; }

        public Ques()
        {
            answers2 = new List<theAnswer>();
        }
    }
}
