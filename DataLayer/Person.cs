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
        public int? CurrentGameId { get; set; }
        public string CatchPhrase { get; set; }

        public double LevelOneWins { get; set; }
        public double LevelTwoWins { get; set; }
        public double LevelThreeWins { get; set; }
        public double LevelOneLose { get; set; }
        public double LevelTwoLose { get; set; }
        public double LevelThreeLose { get; set; }

        public double Answered { get; set; }
        public double DidNotAnswer { get; set; }

        public bool answeredMathQuestion { get; set; }

        public int levelOneAnsweredCorrectly { get; set; }
        public int levelOneAnsweredIncorrectly { get; set; }

        public int levelTwoAnsweredCorrectly { get; set; }
        public int levelTwoAnsweredIncorrectly { get; set; }

        public int levelThreeAnsweredCorrectly { get; set; }
        public int levelThreeAnsweredIncorrectly { get; set; }

        public int overallAnsweredCorrectly { get; set;}

        public int overllAndsweredIncorrectly { get; set; }

        public bool isPlaying { get; set; }
        public bool? assignedBool { get; set; }

        public int? currentMathProblemID { get; set; }
    }
}
