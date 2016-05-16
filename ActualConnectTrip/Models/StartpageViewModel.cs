using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace ActualConnectTrip.Models
{
    public class StartpageViewModel
    {
        public int myid { get; set; }
        public virtual List<startGamePlayer> L1rivals { get; set; }
        public virtual List<string> L1names { get; set; }
        public virtual List<startGamePlayer> L2rivals { get; set; }
        public virtual List<string> L2names { get; set; }
        public virtual List<startGamePlayer> L3rivals { get; set; }
        public virtual List<string> L3names { get; set; }
        public virtual List<startGamePlayer> Recommended { get; set; }
        public virtual List<string> RCnames { get; set; }

        public virtual List<int> RCLevels { get; set; }
        public bool isThereRecommended { get; set; }

        public bool isThereOtherGamesLevel1 { get; set; }

        public bool isThereOtherGamesLevel2 { get; set; }

        public bool isThereOtherGamesLevel3 { get; set; }
        public int? oppoid { get; set;}
        public Boolean? request { get; set; }
        public int gamelevel { get; set; }
    }
}
