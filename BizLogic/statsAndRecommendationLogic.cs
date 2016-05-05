using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic
{
    public static class statsAndRecommendationLogic
    {
        public static double overallPercentage(this Person user)
        {
            return percentage(user.LevelOneWins + user.LevelTwoWins + user.LevelThreeWins, user.LevelOneLose + user.LevelTwoLose + user.LevelThreeLose);
        }

        public static double levelOnePercentage(this Person user)
        {
            return percentage(user.LevelOneWins, user.LevelOneLose);
        }

        public static double levelTwoPercentage(this Person user)
        {
            return percentage(user.LevelTwoWins, user.LevelTwoLose);

        }

        public static double levelThreePercentage(this Person user)
        {
            return percentage(user.LevelThreeWins, user.LevelThreeLose);
        }

        public static double didNotAnwserPercentage(this Person user)
        {
            return percentage(user.DidNotAnswer, user.Answered);
        }

        public static double numGames(this Person user)
        {
            return user.Answered + user.DidNotAnswer;
        }

        public static double numWins(this Person user)
        {
            return user.LevelOneWins + user.LevelTwoWins + user.LevelThreeWins;
        }

        public static double numLose(this Person user)
        {
            return user.LevelOneLose + user.LevelTwoLose + user.LevelThreeLose;
        }
        public static double percentage(double one, double two)
        {
            return (one / (one + two)) * 100;
        }

        public static Person findMatch(this Person user, Entities Context)
        {

            double level1Percentage = levelOnePercentage(user);
            double level2Percentage = levelTwoPercentage(user);
            double level3Percentage = levelThreePercentage(user);
            
            

            
                var possUsers = (from u in Context.Users select u);
                if (level1Percentage >= level2Percentage && level1Percentage >= level3Percentage)
                {

                    possUsers = (from u in Context.Users where Math.Abs(percentage(user.LevelOneWins, user.LevelOneLose) - percentage(u.LevelOneWins, u.LevelOneLose)) <= 30 && Math.Abs(overallPercentage(user) - overallPercentage(u)) <= 50 select u);
                }
                else if (level2Percentage >= level1Percentage && level2Percentage >= level3Percentage)
                {
                    possUsers = (from u in Context.Users where Math.Abs(percentage(user.LevelTwoWins, user.LevelTwoLose) - percentage(u.LevelTwoWins, u.LevelTwoLose)) <= 30 && Math.Abs(overallPercentage(user) - overallPercentage(u)) <= 50 select u);
                }
                else
                {
                    possUsers = (from u in Context.Users where Math.Abs(percentage(user.LevelThreeWins, user.LevelThreeLose) - percentage(u.LevelThreeWins, u.LevelTwoLose)) <= 30 && Math.Abs(overallPercentage(user) - overallPercentage(u)) <= 50 select u);
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

        public static string GameCompliment(this Person user)
        {
            double wins = overallPercentage(user);
            string compliment = null;
            if(wins >=75.00)
            {
                compliment = "You are doing great! Kepp it up!";
            }
            else if(wins >=50.00 && wins <75.00)
            {
                compliment = "You are doing good!";
            }
            else if(wins >=24.00 && wins <50.00)
            {
                compliment = "You can do better!";
            }
            else
            {
                compliment = "You need to put more effort!";
            }
            return compliment;

        }

        public static string MathCompliment(this Person user)
        {
            double lost = didNotAnwserPercentage(user);
            string compliment = null;
            if (lost >= 75.00)
            {
                compliment = "You need to put more effort!";
               
            }
            else if (lost >= 50.00 && lost < 75.00)
            {
                compliment = "You can do better!";
                
            }
            else if (lost >= 24.00 && lost < 50.00)
            {
                compliment = "You are doing good!";
            }
            else
            {
                compliment = "You are doing great! Kepp it up!";
            }
            return compliment;
        }
    }
}

