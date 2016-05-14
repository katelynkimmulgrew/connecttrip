using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActualConnectTrip.Models
{
    public class PracticeMathViewModel
    {
        public string mathQuestion { get; set; }

        public string mathAnswer { get; set; }

        public string userAnswer { get; set; }

        public int levelchosen { get; set; }

        public bool isSelectLevelBlockVisable { get; set; }

        public bool isAnswerBlockVisable { get; set; }

        public string warning {get; set;}

        public bool isNextQuesitonBlockVisable { get; set; }

        public bool isTryAgainBlockVisable { get; set; }

        public bool isTryAgain { get; set; }

        public bool isShowAnswer { get; set; }
    }
}