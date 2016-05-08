using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
   
    public class ForumContext:DbContext
    {
        public virtual DbSet<Ques> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }

        public Ques Getquesforid(int id)
        {
            return (from b in Questions
                    where b.Id == id
                    select b).FirstOrDefault();
        }
    }
}
