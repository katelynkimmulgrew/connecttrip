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

        public bool isSelectLevelVisable { get; set; }

        public bool isAnswerAreaVisable { get; set; }

        public string warning {get; set;}
    }
}