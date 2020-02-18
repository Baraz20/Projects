using System;
using System.Threading;

namespace Sudoku
{
    internal class Program
    {
        private static int len = 0;
        private static int sqrt = 0;
        private static int c = 0;
        private static int wait = 0;

        private static Board VaildLen(int L)
        {
            while (L < 0 || Math.Pow((int)Math.Sqrt(L), 2) != L)
            {
                Console.WriteLine("Enter a number that is postive and have a whole sqrt");
                L = int.Parse(Console.ReadLine());
                Console.Clear();
            }
            len = L;
            sqrt = (int)Math.Sqrt(L);
            return new Board(len, sqrt);
        }

        private static Board ValidBorad(Board b)
        {
            int[,] arr = b.board;
            int L = arr.GetLength(0);
            if (L != arr.GetLength(1))
            {
                Console.WriteLine("Invaild size for Borad - Board must have same highet and width");
                return null;
            }
            if (Math.Pow((int)Math.Sqrt(L), 2) != L)
            {
                Console.WriteLine("Invaild size for Board - Board must have a whole sqrt");
                return null;
            }
            len = L;
            sqrt = (int)Math.Sqrt(len);
            b = new Board(len, sqrt);
            b.board = arr;
            return b;
        }

        private static bool SudokuSolver(int L)
        {
            Board b = VaildLen(L);
            return SudokuSolver(b);
        }

        private static bool SudokuSolver(int[,] arr)
        {
            int L = arr.GetLength(0);
            if (L != arr.GetLength(1))
            {
                Console.WriteLine("Invaild size for Borad - Board must have same highet and width");
                return false;
            }
            if (Math.Pow((int)Math.Sqrt(L), 2) != L)
            {
                Console.WriteLine("Invaild size for Board - Board must have a whole sqrt");
                return false;
            }
            len = L;
            sqrt = (int)Math.Sqrt(len);
            Board b = new Board(len, sqrt);
            b.board = arr;
            return SudokuSolver(b);
        }

        private static bool SudokuSolver(Board b = null)
        {
            if (b == null)
            {
                len = b.board.GetLength(0);
                b = VaildLen(len);
            }
            else
            {
                b = ValidBorad(b);
            }
            if (!ReSudoku(b))
            {
                Console.WriteLine("No Solotion");
                return false;
            }
            Console.WriteLine("Did it");
            return true;
        }

        private static bool ReSudoku(Board b)
        {
            bool isFree = false;
            int row = -1;
            int col = -1;
            int[,] board = b.board;

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (board[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        isFree = true;
                        break;
                    }
                }
                if (isFree)
                {
                    break;
                }
            }
            if (!isFree)
            {
                c++;
                Console.WriteLine(c + "-");
                Console.WriteLine(b);
                return true;
            }
            bool chk = false;
            for (int num = 1; num <= len; num++)
            {
                if (IsLegal(row, col, num, b))
                {
                    board[row, col] = num;
#if false
                    wait++;
                    if (wait <= 50)
                    {
                        Console.WriteLine(wait);
                        b.Pointer(col, row);
                        Thread.Sleep(10);
                        Console.Clear();
                    }
                    if (wait == 100000)
                    {
                        wait = 0;
                    }
#endif
                    chk = ReSudoku(b) || chk;

                    b.board[row, col] = 0;
                }
            }
            return chk;
        }

        private static bool IsLegal(int row, int col, int num, Board b)
        {
            int[,] board = b.board;
            int boxRow = row - row % sqrt;
            int boxCol = col - col % sqrt;
            if (board[row, col] != 0)
            {
                return false;
            }
            for (int i = 0; i < len; i++)
            {
                if (board[i, col] == num || board[row, i] == num)
                {
                    return false;
                }
            }
            for (int i = 0; i < sqrt; i++)
            {
                for (int j = 0; j < sqrt; j++)
                {
                    if (board[i + boxRow, j + boxCol] == num)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static Board Editor(int L)
        {
            return Editor(VaildLen(L));
        }

        private static Board Editor(Board b = null)
        {
            if (b != null)
            {
                b = ValidBorad(b);
            }
            bool chk1 = false;
            bool chk2 = false;
            int input = -1;
            int left = 0;
            int top = 0;
            int size = len;
            bool stop = true;
            while (stop)
            {
                Console.Clear();
                Console.WriteLine("\n\rPress 'ESC' to stop");
                Console.WriteLine("\rPress 'Enter' to edit\n");
                b.Pointer(left, top);
                if (chk2)
                {
                    b.SaveBoard();
                    chk2 = false;
                }
                if (chk1)
                {
                    chk1 = false;
                    input = -1;
                    Console.WriteLine("Illegal Move");
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
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
                        stop = false;
                        break;

                    case (ConsoleKey.S):
                        chk2 = true;
                        break;

                    case (ConsoleKey.Enter):
                        while (input <= -1 || input > len)
                        {
                            Console.WriteLine("Enter a number");
                            Console.Write("Input: ");
                            input = int.Parse(Console.ReadLine());
                        }
                        if (input == 0 || IsLegal(top, left, input, b))
                        {
                            b.board[top, left] = input;
                            input = -1;
                        }
                        else
                        {
                            chk1 = true;
                        }
                        break;

                    default:
                        if (char.IsNumber(keyInfo.KeyChar))
                        {
                            int num = int.Parse(keyInfo.KeyChar.ToString());
                            Console.WriteLine(num);
                            if (num == 0 || IsLegal(top, left, num, b) && num > 0 && num <= len)
                            {
                                b.board[top, left] = num;
                            }
                        }
                        break;
                }
            }
            return b;
        }

        private static void Main(string[] args)
        {
            int[,] board =
            {
                {8, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 3, 6, 0, 0, 0, 0, 0},
                {0, 7, 0, 0, 9, 0, 2, 0, 0},
                {0, 5, 0, 0, 0, 7, 0, 0, 0},
                {0, 0, 0, 0, 4, 5, 7, 0, 0},
                {0, 0, 0, 1, 0, 0, 0, 3, 0},
                {0, 0, 1, 0, 0, 0, 0, 6, 8},
                {0, 0, 8, 5, 0, 0, 0, 1, 0},
                {0, 9, 0, 0, 0, 0, 4, 0, 0}
            };
            //SudokuSolver(b);
            //SudokuSolver(4);
            //SudokuSolver();
            Board b = new Board(board.GetLength(0));
            b.board = board;
            SudokuSolver(Editor(9));
        }
    }
}