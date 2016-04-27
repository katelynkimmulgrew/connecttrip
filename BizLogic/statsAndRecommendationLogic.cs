using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic
{
    public class statsAndRecommendationLogic
    {
        public static double overallPercentage(this User user)
        {
            return percentage(user.LevelOneWins + user.LevelTwoWins + user.LevelThreeWins, user.LevelOneLose + user.LevelTwoLose + user.LevelThreeLose);
        }

        public static double levelOnePercentage(this User user)
        {
            return percentage(user.LevelOneWins, user.LevelOneLose);
        }

        public static double levelTwoPercentage(this User user)
        {
            return percentage(user.LevelTwoWins, user.LevelTwoLose);

        }

        public static double levelThreePercentage(this User user)
        {
            return percentage(user.LevelThreeWins, user.LevelThreeLose);
        }

        public static double didNotAnwserPercentage(this User user)
        {
            return percentage(user.DidNotAnswer, user.Answered);
        }

        public static double numGames(this User user)
        {
            return user.Answered + user.DidNotAnswer;
        }

        public static double numWins(this User user)
        {
            return user.LevelOneWins + user.LevelTwoWins + user.LevelThreeWins;
        }

        public static double numLose(this User user)
        {
            return user.LevelOneLose + user.LevelTwoLose + user.LevelThreeLose;
        }
        public static double percentage(double one, double two)
        {
            return (one / (one + two)) * 100;
        }

        public static User findMatch(this User user)
        {

            double level1Percentage = levelOnePercentage(user);
            double level2Percentage = levelTwoPercentage(user);
            double level3Percentage = levelThreePercentage(user);
            using (var Context = new Entities())
            {
                var possUsers = (from u in Context.Users select u);
                if (level1Percentage >= level2Percentage && level1Percentage >= level3Percentage)
                {

                    possUsers = (from u in Context.Users where Math.Abs(percentage(user.LevelOneWins, user.LevelOneLose) - percentage(u.levelOneWins, u.levelOneLose)) <= 30 && Math.Abs(overallPercentage(user) - overallPercentage(u)) <= 50 select u);
                }
                else if (level2Percentage >= level1Percentage && level2Percentage >= level3Percentage)
                {
                    possUsers = (from u in Context.Users where Math.Abs(percentage(user.LevelTwoWins, user.LevelTwoLose) - percentage(u.levelTwoeWins, u.levelTwoLose)) <= 30 && Math.Abs(overallPercentage(user) - overallPercentage(u)) <= 50 select u);
                }
                else
                {
                    possUsers = (from u in Context.Users where Math.Abs(percentage(user.LevelThreeWins, user.LevelThreeLose) - percentage(u.levelThreeWins, u.levelTwoLose)) <= 30 && Math.Abs(overallPercentage(user) - overallPercentage(u)) <= 50 select u);
                }

                Random rand = new Random();
                int toSkip = rand.Next(0, possUsers.Count());

                var chosenUser = possUsers.Skip(toSkip).Take(1).FirstOrDefault();
                while (chosenUser == user)
                {
                    toSkip = rand.Next(0, Context.Users.Count());

                    chosenUser = Context.Users.Skip(toSkip).Take(1).FirstOrDefault();
                }

                if (chosenUser == null)
                {
                    chosenUser = user;
                    while (chosenUser == user)
                    {
                        int toSkip2 = rand.Next(0, Context.Users.Count());
                        chosenUser = Context.Users.Skip(toSkip2).Take(1).FirstOrDefault();
                    }
                }


                return chosenUser;
            }

        }
    }
}
}
