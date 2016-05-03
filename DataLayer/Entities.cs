
using EntityFramework.Triggers;
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
        public override Int32 SaveChanges()
        {
            return this.SaveChangesWithTriggers();
        }
        
        public virtual DbSet<startGamePlayer> startGamePlayers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Row> Rows { get; set; }

        public virtual DbSet<Column> Columns { get; set; }
        public virtual DbSet<mathProblemResult> Result { get; set; }

        public virtual DbSet<Ques> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }

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
            int p = 0;
            return (from c in game.Grid
                    where c.ColumnNumber == colNo
                    select c).FirstOrDefault();
        }
    }
}
