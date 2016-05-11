using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic
{
        public static class ConnectTripLogic
        {

        
        public static bool?[,] switchToBools(this Game board, Entities db)
        {
            
            bool?[,] array = new bool?[board.maxRows, board.maxCols];

            int i, j;
            for(i=0;i<board.maxCols;i++)
            {
                for(j=0;j<board.maxRows;j++)
                {
                    Column currentCol = db.getCol(i, board);
                    Row currentRow = db.getRow(currentCol, j);
                    array[j, i] = currentRow.Value;
                }
            }
            return array;
            
        }

        public static bool isFull(this Game board, Entities db)
        {
            bool?[,] array = switchToBools(board, db);
            
            for(int i = 0; i<board.maxRows;i++)
            {
                for(int j=0;j<board.maxCols;j++)
                {
                    if(array[i,j]==null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void SwitchPlayers(this Game board)
        {
            if (board != null)
                board.currentUser = (board.currentUser == true) ? false : true;
        }
    
        
            public static Game setBoard()
            {
                Game game = new Game();

            game.maxCols = 6;
            game.maxRows = 7;

                for (int i = 1; i <= game.maxRows; i++)
                {
                    Column column = new Column();
                    column.ColumnNumber = i;
                    for (int j = 1; j <= game.maxCols; j++)
                    {
                        Row row = new Row { RowNumber = j, Value = null };
                        column.Rows.Add(row);

                    }
                    game.Grid.Add(column);
                
                
                }

                return game;

            }

            public static Row determinePlace(this Game game, bool player, int columnNumber, Entities Context)
            {
                Column determineColumn = Context.getCol(columnNumber, game);
                foreach (var row in determineColumn.Rows)
                {
                    if (row.Value == null)
                    {
                        row.Value = player;
                    Context.SaveChanges();
                        return row;
                    }

                }
                return null;
            }

            public static bool determineWin(this Game game, Entities Context, Row row, Column col)
            {
                bool a = checkHoriz(game, Context, row, col);
                bool b = checkVert(game, Context, row, col);
                bool c = checkDiag(game, Context, row, col, "left");
                bool d = checkDiag(game, Context, row, col, "right");
                return (a || b || c || d);
            }

            public static bool checkHoriz(this Game game, Entities Context, Row row, Column col)
            {
                int colNo = col.ColumnNumber;
                int rowNo = row.RowNumber;
                int count = 0;
                int currentCol = colNo;
                bool? typePlayer = row.Value;
                while (currentCol >= 0)
                {
                    Row tempRow = Context.getRow(Context.getCol(currentCol - 1, game), rowNo);
                    if (tempRow.Value == typePlayer)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                    currentCol -= 1;
                }
                currentCol = colNo;
                while (currentCol < game.maxCols)
                {
                    Row tempRow = Context.getRow(Context.getCol(currentCol + 1, game), rowNo);
                    if (tempRow.Value == typePlayer)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                    currentCol += 1;
                }
                return count >= 4;
            }

            public static bool checkVert(this Game game, Entities Context, Row row, Column col)
            {
                int colNo = col.ColumnNumber;
                int rowNo = row.RowNumber;
                int count = 0;
                int currentRow = rowNo;
                bool? typePlayer = row.Value;
                while (currentRow >= 0)
                {
                    Row tempRow = Context.getRow(Context.getCol(colNo, game), rowNo - 1);
                    if (tempRow.Value == typePlayer)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                    currentRow -= 1;
                }
                currentRow = rowNo;
                while (currentRow < game.maxRows)
                {
                    Row tempRow = Context.getRow(Context.getCol(colNo, game), rowNo + 1);
                    if (tempRow.Value == typePlayer)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                    currentRow += 1;
                }
                return count >= 4;
            }

            public static bool checkDiag(this Game game, Entities Context, Row row, Column col, string direction)
            {
            
                int colNo = col.ColumnNumber;
                int rowNo = row.RowNumber;
                int count = 0;
                int currentCol = colNo;
                int currentRow = rowNo;
                bool? typePlayer = row.Value;
                if (direction == "left")
                {
                    while (currentCol < game.maxCols && currentRow >= 0)
                    {
                        Row tempRow = Context.getRow(Context.getCol(currentCol - 1, game), currentRow + 1);
                        if (tempRow.Value == typePlayer)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                        currentCol -= 1;
                        currentRow += 1;
                    }
                    currentCol = colNo;
                    currentRow = rowNo;
                    while (currentCol < game.maxCols && currentRow >= 0)
                    {
                        Row tempRow = Context.getRow(Context.getCol(currentCol + 1, game), currentRow - 1);
                        if (tempRow.Value == typePlayer)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                        currentCol += 1;
                        currentRow -= 1;
                    }
                    return count >= 4;
                }
                else
                {
                    while (currentCol >= 0 && currentRow >= 0)
                    {
                        Row tempRow = Context.getRow(Context.getCol(currentCol - 1, game), currentRow - 1);
                        if (tempRow.Value == typePlayer)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                        currentCol -= 1;
                        currentRow -= 1;
                    }
                    currentCol = colNo;
                    currentRow = rowNo;
                    while (currentCol < game.maxCols && currentRow < game.maxRows)
                    {
                        Row tempRow = Context.getRow(Context.getCol(currentCol + 1, game), currentRow - 1);
                        if (tempRow.Value == typePlayer)
                        {
                            count++;
                        }
                        else
                        {
                            break;
                        }
                        currentCol += 1;
                        currentRow += 1;
                    }
                    return count >= 4;
                }
            }



        }
}

