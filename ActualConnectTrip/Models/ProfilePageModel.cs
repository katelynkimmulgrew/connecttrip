using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActualConnectTrip.Models
{
    public class ProfilePageModel
    {
        public string userName { get; set; }
        public string catchphraseView { get; set; }
        public double overAllPercentageView { get; set; }
        public double levelOnePercentageView { get; set; }
        public double levelTwoPercentageView { get; set; }
        public double levelThreePercentageView { get; set; }
        
        public double totalNumberOfWins { get; set; }
        public double totalNumberOfLose { get; set; }
        
    }
}