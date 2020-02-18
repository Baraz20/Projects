using System;

namespace Sudoku
{
    public class Board
    {
        public int size { get; }
        public int split { get; }
        public int[,] board { get; set; }

        public Board(int size, int split = 0)
        {
            if (size > 0)
            {
                this.size = size;
            }
            else
            {
                Console.WriteLine("Error: illegal size");
            }
            this.split = split;
            if (split > 0)
            {
                if (size % split == 0)
                {
                    MakeEmptyBoard();
                }
                else
                {
                    Console.WriteLine("Error: size and split don't match!");
                }
            }
            else
            {
                MakeEmptyBoard();
            }
        }

        public void MakeEmptyBoard()
        {
            this.board = new int[this.size, this.size];
            for (int row = 0; row < this.size; row++)
            {
                for (int col = 0; col < this.size; col++)
                {
                    this.board[row, col] = 0;
                }
            }
        }

        public void SaveBoard()
        {
            for (int i = 0; i < this.size; i++)
            {
                Console.Write("{");
                for (int j = 0; j < this.size; j++)
                {
                    Console.Write(board[i, j]);
                    if (j != size - 1)
                        Console.Write(", ");
                }
                Console.Write("}");
                if (i != size - 1)
                    Console.Write(",");
                Console.Write("\n");
            }
        }

        public void Pointer(int left, int top)
        {
            int cal = (left + top * this.size) + 1;
            int c = 0;
            bool doSplit = this.split != 0;
            if (doSplit)
            {
                int jump = this.size / this.split;
            }
            for (int row = 0; row < this.size; row++)
            {
                for (int col = 0; col < this.size; col++)
                {
                    c++;
                    if (c == cal)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        if (this.board[row, col] == 0)
                        {
                            Console.Write(" . ");
                        }
                        else
                        {
                            Console.Write($" {this.board[row, col]} ");
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        if (this.board[row, col] == 0)
                        {
                            Console.Write(" . ");
                        }
                        else
                        {
                            Console.Write($" {this.board[row, col]} ");
                        }
                    }
                    if (doSplit && col + 1 != this.size && (col + 1) % this.split == 0)
                    {
                        Console.Write("|");
                    }
                }
                Console.Write("\n");
                if (doSplit && row + 1 != this.size && (row + 1) % this.split == 0)
                {
                    for (int i = 0; i < this.size; i++)
                    {
                        Console.Write("---");
                        if (i + 1 != this.size && (i + 1) % this.split == 0)
                        {
                            Console.Write("+");
                        }
                    }
                    Console.Write("\n");
                }
            }
            Console.Write("\n");
        }

        public override string ToString()
        {
            string S = string.Empty;
            bool doSplit = this.split != 0;
            if (doSplit)
            {
                int jump = this.size / this.split;
            }
            for (int row = 0; row < this.size; row++)
            {
                for (int col = 0; col < this.size; col++)
                {
                    if (this.board[row, col] == 0)
                    {
                        S += $" . ";
                    }
                    else
                    {
                        S += $" {this.board[row, col]} ";
                    }
                    if (doSplit && col + 1 != this.size && (col + 1) % this.split == 0)
                    {
                        S += "|";
                    }
                }
                S += "\n";
                if (doSplit && row + 1 != this.size && (row + 1) % this.split == 0)
                {
                    for (int i = 0; i < this.size; i++)
                    {
                        S += "---";
                        if (i + 1 != this.size && (i + 1) % this.split == 0)
                        {
                            S += "+";
                        }
                    }
                    S += "\n";
                }
            }
            return S;
        }
    }
}