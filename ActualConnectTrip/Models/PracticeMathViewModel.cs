using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActualConnectTrip.Models
{
    public class PracticeMathViewModel
    {
        public string mathQuestion { get; set; }

        public int mathAnswer { get; set; }

        public int userAnswer { get; set; }

        public int levelchosen { get; set; }

        public bool isSelectLevelVisable { get; set; }
    }
}