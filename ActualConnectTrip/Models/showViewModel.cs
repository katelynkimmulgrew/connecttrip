using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualConnectTrip.Models
{
    public class showViewModel
    {
        public IEnumerable<DataLayer.Ques> model1 { get; set; }
        public DataLayer.Ques model2 { get; set; }
        public IEnumerable<DataLayer.Answer> model3 { get; set; }
        public DataLayer.Answer model4 { get; set; }
        public int model5 { get; set; }
    }
}
