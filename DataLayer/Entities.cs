
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Entities : DbContext
    {
        
        
        public virtual DbSet<startGamePlayer> startGamePlayers { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Row> Rows { get; set; }

        public virtual DbSet<Column> Columns { get; set; }
        public virtual DbSet<mathProblemResult> Result { get; set; }

        public virtual DbSet<Ques> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }

        public virtual DbSet<mathProblemResult> mathProblemResults { get; set; }

        public mathProblemResult getmathProblemResultById(int id)
        {
            
            return (from p in mathProblemResults
                    where p.Id == id
                    select p).FirstOrDefault();
        
    }
        

        public Ques Getquesforid(int id)
        {
            return (from b in Questions
                    where b.Id == id
                    select b).FirstOrDefault();
        }
        public Row getRow(Column col, int rowNo)
        {

            return (from r in col.Rows
                    where r.RowNumber == rowNo
                    select r).FirstOrDefault();
        }

        public Column getCol(int colNo, Game game)
        {
            return (from c in game.Grid
                    where c.ColumnNumber == colNo
                    select c).FirstOrDefault();
        }

        public Game getGameById(int id)
        {
            return (from g in Games where g.Id == id select g).FirstOrDefault();
        }

        public Person getPersonById(int id)
        {
            return (from p in Persons where p.Id == id select p).FirstOrDefault();
        }
    }
}
