using System;
using System.Threading;

namespace RatInMaze
{
    internal class Program
    {
        private static int len = 0;
        private static int step = 1;
        private static int c = 0;

        private static int minStep = 0;
        private static int tempC = 0;

        private static void MarkPath(Board board, Board solution)
        {
            ConsoleColor color = ConsoleColor.Red;
            bool[,] b = board.board;
            bool[,] sol = solution.board;
            bool chk = false; // this is not neccesry just to make it quicker
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (b[i, j] && sol[i, j])
                    {
                        Console.BackgroundColor = color;
                        chk = true;
                    }
                    if (b[i, j])
                        Console.Write(" . ");
                    else
                        Console.Write(" # ");
                    if (chk)

                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static Board Editor(bool[,] arr)
        {
            Board b = null;
            if (arr == null || arr.GetLength(0) < 0)
            {
                Console.WriteLine("Invaild array in the editor");
                b = ValidBorad(b);
                return Editor(b);
            }

            b = new Board(arr.GetLength(0));
            b.board = arr;
            return Editor(b);
        }

        private static Board Editor(int num)
        {
            num = ValidNum(num);
            Board b = new Board(num);
            return Editor(b);
        }

        private static Board Editor(Board b)
        {
            bool mark = true;
            b = ValidBorad(b);
            b.board[0, 0] = true;
            b.board[len - 1, len - 1] = true;
            int left = 0;
            int top = 0;
            int size = len;
            bool stop = false;
            while (!stop)
            {
                Console.WriteLine("Press 'ESC' to stop");
                Console.WriteLine("Press 'Enter' to switch between paths and walls");
                Console.WriteLine("Press 'Spacebar' to place");
                Console.WriteLine("Press 'S' to print board as boolean array");
                Console.WriteLine($"Start:(0,0) ; Destination:({len - 1},{len - 1})\n");

                Console.WriteLine(left + " " + top);

                b.Pointer(left, top);
                string S = mark ? "Path" : "Wall";
                Console.Write("\nYou are placing: " + S);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                Console.Clear();

                switch (keyInfo.Key)
                {
                    case (ConsoleKey.DownArrow):
                        if (top < size - 1) top++;
                        break;

                    case (ConsoleKey.UpArrow):
                        if (top > 0) top--;
                        break;

                    case (ConsoleKey.RightArrow):
                        if (left < size - 1) left++;
                        break;

                    case (ConsoleKey.LeftArrow):
                        if (left > 0) left--;
                        break;

                    case (ConsoleKey.Escape):
                        stop = true;
                        break;

                    case (ConsoleKey.S):
                        b.Save();
                        break;

                    case (ConsoleKey.Spacebar):
                        if (!((top == 0 && left == 0) || (top == len - 1 && left == len - 1)))
                        {
                            b.board[top, left] = mark;
                        }
                        else
                            Console.WriteLine("Can't change starting postion or the destination");
                        break;

                    case (ConsoleKey.Enter):
                        mark = !mark;
                        break;

                    default:
                        break;
                }
            }
            return b;
        }

        private static Board ValidBorad(Board b)
        {
            if (b != null)
            {
                len = b.board.GetLength(0);
                return b;
            }
            int L = 0;
            L = ValidNum(L);
            len = L;
            return new Board(L);
        }

        private static int ValidNum(int num)
        {
            while (num >= len && num <= 0)
            {
                Console.WriteLine("Enter a postive number for the postion of the destination");
                num = int.Parse(Console.ReadLine());
            }
            return num;
        }

        private static bool BeginMaze(bool[,] arr)
        {
            Board b = null;
            if (arr == null || arr.GetLength(0) < 0)
            {
                Console.WriteLine("invaild array size");
                b = ValidBorad(b);
                return BeginMaze(b);
            }
            b = new Board(arr.GetLength(0));
            b.board = arr;
            return BeginMaze(b);
        }

        private static bool BeginMaze(int L)
        {
            while (L <= 0)
            {
                Console.WriteLine("Enter a postive number");
                L = int.Parse(Console.ReadLine());
            }
            len = L;
            return BeginMaze(new Board(len));
        }

        private static bool BeginMaze(Board b)
        {
            b = ValidBorad(b);
            Board sol = new Board(len);
            b.board[0, 0] = true;
            b.board[len - 1, len - 1] = true;
            sol.board[len - 1, len - 1] = true;
            minStep = len * len;
            if (!SolveMaze(b, sol, 0, 0))
            {
                Console.WriteLine("No solotion");

                return false;
            }
            Console.WriteLine($"\nThe Shortest path is #{tempC} with {minStep} steps");
            return true;
        }

        private static bool IsLegal(Board b, Board sol, int row, int col)
        {
            return row >= 0 && row < len && col >= 0 && col < len && b.board[row, col] && !sol.board[row, col];
        }

        //Up - U ; Down - D ; Left - L ; Right - R
        private static bool SolveMaze(Board b, Board sol, int row, int col)
        {
            if (row == len - 1 && col == len - 1)
            {
                c++;
                if (step < minStep)
                {
                    minStep = step;
                    tempC = c;
                }
                Console.WriteLine($"\nsteps: {step} ; #{c}- ");

                MarkPath(b, sol);
#if false //make both #if true to see the progress
                Thread.Sleep(1000);
#endif
                return true;
            }
            bool chk = false;
            if (IsLegal(b, sol, row, col))
            {
                step++;
                sol.board[row, col] = true;
#if false
                int time = 100; // miliseconds (1000 milisec = 1 sec)
                Console.WriteLine($"{row} {col}");
                Console.WriteLine("Solution:");
                sol.Pointer(col, row);

                Console.WriteLine("Maze:");
                b.Pointer(col, row);
                Thread.Sleep(time);
                Console.Clear();
#endif
                chk = SolveMaze(b, sol, row, col + 1) || chk;
                chk = SolveMaze(b, sol, row - 1, col) || chk;
                chk = SolveMaze(b, sol, row + 1, col) || chk;
                chk = SolveMaze(b, sol, row, col - 1) || chk;

                sol.board[row, col] = false;
                step--;
            }
            return chk;
        }

        private static void Main(string[] args)
        {
            bool[,] arr =
            {
                {true,false,false,false,false,false,false,false,false,false},
                {true,true,true,true,false,true,false,true,true,true},
                {false,true,false,true,true,true,true,true,false,false},
                {false,true,true,true,false,false,false,true,true,true},
                {false,true,false,true,true,true,true,true,false,true},
                {false,true,false,true,false,false,false,true,false,true},
                {false,true,false,true,false,true,true,true,false,true},
                {false,true,false,true,false,false,false,true,false,true},
                {false,true,true,true,true,true,false,true,true,true},
                {false,false,false,false,false,false,false,false,false,true}
            };
            Board b = new Board(9);
            int L = b.board.GetLength(0);
            for (int i = 0; i < L; i++)
            {
                for (int j = 0; j < L; j++)
                {
                    b.board[i, j] = true;
                }
            }
            b.board = arr;
            Console.WriteLine();
            BeginMaze(Editor(arr));
        }
    }
}