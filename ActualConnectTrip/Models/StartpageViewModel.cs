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
        public virtual List<startGamePlayer> rivals { get; set; }

        public int oppoid { get; set;}
    }
}
