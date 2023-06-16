using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;

namespace ConsoleApp1
{
    class Generator
    {
        public int counter = 0;
        public int solution = 0;
        public int[,] startingGrid = {
                    { 0,0,0,0 },
                    { 0,0,0,0 },
                    { 0,0,0,0 },
                    { 0,0,0,0 }
                };
        public List<int[,]> grids = new List<int[,]>();
        public Generator()
        {
            Console.WriteLine("+++++++ GENERATE +++++++");
            generate(startingGrid);

            // Save resolved grids in an Excel file
            ExportArrayToExcel(grids, "/resolved.xlsx");

            Console.WriteLine("+++++++ REMOVE +++++++");
            for (int i = 0; i < grids.Count; i++)
            {
                Console.WriteLine("==> GRID " + i + " : ");
                Console.WriteLine("+++++++ BEFORE +++++++");
                Print(grids[i]);
                Remove(grids[i]);
                Console.WriteLine("+++++++ AFTER +++++++");
                Print(grids[i]);
            }

            // Save unresolved grids in an Excel file
            ExportArrayToExcel(grids, "/unresolved.xlsx");
        }
        public void generate(int[,] grid)
        {
            if (CheckGrid(grid))
            {
                counter++;
                int[,] copyGrid = new int[4, 4];
                for (int i = 0; i < 16; i++)
                {
                    int rc = i / 4;
                    int cc = i % 4;
                    copyGrid[rc, cc] = grid[rc, cc];
                }
                grids.Add(copyGrid);
                return;
            }

            for (int i = 0; i < 16; i++)
            {
                int r = i / 4;
                int c = i % 4;

                if (grid[r, c] == 0)
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
        }

        public bool CheckGrid(int[,] grid)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (grid[i, j] == 0)
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
            Console.WriteLine("===================================");
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Console.Write("|");
                    Console.Write(grid[r, c]);
                    Console.Write("|");
                }
                Console.WriteLine("");
            }
        }
        public void Solve(int[,] grid)
        {
            if (CheckGrid(grid))
            {
                this.solution += 1;
                return;
            }
            for (int i = 0; i < 16; i++)
            {
                int r = i / 4;
                int c = i % 4;
                if (grid[r, c] == 0)
                {
                    for (int value = 1; value <= 4; value++)
                    {
                        if (!Used(value, r, c, grid))
                        {
                            grid[r, c] = value;
                            Solve(grid);
                            grid[r, c] = 0;

                        }
                    }
                    return;
                }
            }
        }
        public void Remove(int[,] grid)
        {
            Random random = new Random();
            int attempts = 2;
            while (attempts > 0)
            {
                int r = random.Next(0, 4);
                int c = random.Next(0, 4);
                while (grid[r, c] == 0)
                {
                    r = random.Next(0, 4);
                    c = random.Next(0, 4);
                }
                int backup = grid[r, c];
                grid[r, c] = 0;

                int[,] copyGrid = new int[4, 4];
                for (int i = 0; i < 16; i++)
                {
                    int rc = i / 4;
                    int cc = i % 4;
                    copyGrid[rc, cc] = grid[rc, cc];
                }

                solution = 0;
                this.Solve(copyGrid);
                if (solution != 1)
                {
                    grid[r, c] = backup;
                    attempts -= 1;
                }
            }
        }

        public void ExportArrayToExcel(List<int[,]> arrays, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Create a new worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Write the array values to the worksheet
                for (int x = 0; x < arrays.Count; x++)
                {
                    for (int i = 0; i < arrays[x].Length; i++)
                    {
                        for (int j = 0; j < arrays[x].Length; j++)
                        {
                            worksheet.Cells[i, j + (x * 5)].Value = arrays[x][i, j];
                        }

                    }
                }


                // Save the Excel package to a file
                FileInfo excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            new Generator();
        }
    }
}