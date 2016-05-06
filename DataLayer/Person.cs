using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public int CurrentGameId { get; set; }
        public string CatchPhrase { get; set; }

        public double LevelOneWins { get; set; }
        public double LevelTwoWins { get; set; }
        public double LevelThreeWins { get; set; }
        public double LevelOneLose { get; set; }
        public double LevelTwoLose { get; set; }
        public double LevelThreeLose { get; set; }

        public double Answered { get; set; }
        public double DidNotAnswer { get; set; }
    }
}
