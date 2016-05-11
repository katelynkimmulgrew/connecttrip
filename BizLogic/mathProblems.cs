using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic
{
    public class mathProblems
    {
        public int Level { get; set; }
        public char[] operatorList = { '+', '-', '*' };
        StringBuilder builder = new StringBuilder();
        Random random = new Random();

        public string mathQuestion(int level)
        {
            int op1 = 0;
            int op2 = 0;
            char? operand = null;
            Level = level;
            if (Level == 1)
            {
                op1 = random.Next(1, 9);
                operand = operatorList[random.Next(0, level - 1)];
                op2 = random.Next(1, 9);
            }
            else if (Level == 2)
            {
                op1 = random.Next(10, 19);
                operand = operatorList[random.Next(0, level - 1)];
                op2 = random.Next(10, 19);
            }
            else if (Level == 3)
            {
                op1 = random.Next(20, 29);
                operand = operatorList[random.Next(0, level - 1)];
                op2 = random.Next(20, 29);
            }

            if (operand == '-')
            {
                if (op1 < op2)
                {
                    int temp = op1;
                    op1 = op2;
                    op2 = temp;
                }
            }
            builder.Append(op1);
            builder.Append(operand);
            builder.Append(op2);
            string output = builder.ToString();
            //Console.WriteLine("question:" + output);
            return output;
        }

        public string mathAnswer(string question)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("question", typeof(string), question);
            DataRow row = dt.NewRow();
            dt.Rows.Add(row);
            return (string)row["question"];//int.Parse((string)row["question"]);
        }
    }
}

