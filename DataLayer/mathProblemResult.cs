using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class mathProblemResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        public string question { get; set; }

        public string answer { get; set; }

        [Required]
        public Boolean isRight { get; set; }

        [Required]
        public int level { get; set; }

        public bool current { get; set; }

        public DateTime start { get; set; }

 
    }
}
