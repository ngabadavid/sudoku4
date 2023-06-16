using System;

namespace sudoku4
{
    class Generator
    {
        public int counter = 0;
       
        public Generator()
        {

        }
        public void generate(int[,] grid)
        {
            if (CheckGrid(grid))
            {
                counter++;
                Print(grid);
                return;
            }

            for (int i = 0; i < 16; i++)
            {
                int r = i / 4;
                int c = i % 4;

                if(grid[r,c] == 0)
                {
                    for (int num = 1; num <= 4; num++)
                    {
                        if (!Used(num, r, c, grid))
                        {
                            grid[r, c] = num;
                            generate(grid);
                            grid[r, c] = 0;
                        }
                        
                    }
                    return;
                }
            }
            //return true;
        }

        public bool CheckGrid(int[,] grid)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid[i,j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool UsedInRow(int value, int row, int[,] grid)
        {
            for (int col = 0; col < 4; col++)
            {
                if (grid[row, col] == value)
                {
                    return true;
                }
            }

            return false;
        }

        private bool UsedInColumn(int value, int col, int[,] grid)
        {
            for (int row = 0; row < 4; row++)
            {
                if (grid[row, col] == value)
                {
                    return true;
                }
            }

            return false;
        }

        private bool UsedInRegion(int value, int row, int col, int[,] grid)
        {
            int boxStartRow = row - (row % 2);
            int boxStartCol = col - (col % 2);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (grid[boxStartRow + i, boxStartCol + j] == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool Used(int value, int row, int col, int[,] grid)
        {
            return UsedInRow(value, row, grid) || UsedInColumn(value, col, grid) || UsedInRegion(value, row, col, grid);
        }

        public void Print(int[,] grid)
        {
            Console.WriteLine("Grid " + counter + " : =====================================");
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Console.Write("|");
                    Console.Write(grid[r,c]);
                    Console.Write("|");
                }
                Console.WriteLine("");
            }
        }

    }
    class Program
    {
        
        static void Main(string[] args)
        {
            int[,] grid = {
            { 0,0,0,0 },
            { 0,0,0,0 },
            { 0,0,0,0 },
            { 0,0,0,0 }
        };
            Generator g = new Generator();
            g.generate(grid);
        }
    }
}
