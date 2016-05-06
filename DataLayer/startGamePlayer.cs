using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class startGamePlayer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int player1Id { get; set; }
        public int player2Id { get; set; }

        public int level { get; set; }
        public bool isStarted { get; set; }
    }
}
