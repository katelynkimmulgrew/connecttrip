using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLogic
{
        public static class ConnectTripLogic
        {


            public static Game setBoard()
            {
                Game game = new Game();

                for (int i = 1; i <= game.maxRows; i++)
                {
                    Column column = new Column();
                    column.ColumnNumber = i;
                    for (int j = 1; j <= game.maxCols; j++)
                    {
                        Row row = new Row { RowNumber = j, value = null };
                        column.Rows.Add(row);

                    }
                    game.Grid.Add(column);
                }

                return game;

            }

            public static Row determinePlace(this Game game, bool player, int columnNumber, Entities Context)
            {
                Column determineColumn = getColumn(columnNumber, game);
                foreach (var row in determineColumn.Rows)
                {
                    if (row.Value == null)
                    {
                        row.Value = player;
                        return row;
                    }

                }
                return null;
            }

            public static bool determineWin(this Game game, Entities Context, Row row, Column col)
            {
                bool a = checkHoriz(game, context, row, col);
                bool b = checkVert(game, context, row, col);
                bool c = checkDiag(game, context, row, col, "left");
                bool d = checkDiag(game, context, row, col, "right");
                return (a || b || c || d);
            }

            public static bool checkHoriz(this Game game, Entities Context, Row row, Column col)
            {
                int colNo = col.ColumnNumber;
                int rowNo = row.RowNumber;
                int count = 0;
                int currentCol = colNo;
                bool? typePlayer = row.value;
                while (currentCol >= 0)
                {
                    Row tempRow = Context.getRow(Context.getCol(currentCol - 1, game), rowNo);
                    if (tempRow.value == typePlayer)
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
                    if (tempRow.value == typePlayer)
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
                bool? typePlayer = row.value;
                while (currentRow >= 0)
                {
                    Row tempRow = Context.getRow(Context.getCol(colNo, game), rowNo - 1);
                    if (tempRow.value == typePlayer)
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
                    if (tempRow.value == typePlayer)
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
                bool? typePlayer = row.value;
                if (direction == "left")
                {
                    while (currentCol < game.maxCols && currentRow >= 0)
                    {
                        Row tempRow = Context.getRow(Context.getCol(currentCol - 1, game), currentRow + 1);
                        if (tempRow.value == typePlayer)
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
                        if (tempRow.value == typePlayer)
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
                        if (tempRow.value == typePlayer)
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
                        if (tempRow.value == typePlayer)
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
}
