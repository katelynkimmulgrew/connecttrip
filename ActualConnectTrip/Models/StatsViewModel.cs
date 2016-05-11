using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActualConnectTrip.Models
{
    public class StatsViewModel
    {
        public double overAllPercentageView { get; set; }
        public double levelOnePercentageView { get; set; }
        public double levelTwoPercentageView { get; set; }
        public double levelThreePercentageView { get; set; }
        //public double didNotAnswerView { get; set; }
        public double totalNumberOfGames { get; set; }
        public double totalNumberOfWins { get; set; }
        public double totalNumberOfLose { get; set; }
        public string GameComplimentView { get; set; }

        public double levelOneMathPercentage { get; set; }
        public double levelTwoMathPercentage { get; set; }
        public double levelThreeMathPercentage { get; set; }
        public double overAllCorrectAnswersPercentage { get; set; }
        public string MathComplimentView { get; set; }
    }
}