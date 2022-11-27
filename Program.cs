using System;
using System.Threading;

namespace CaroGame
{
    public class ConsoleSpinner
    {
        static string[,] sequence = { };
        public int Delay { get; set; } = 200;
        int totalSequences = 0;
        int counter;
        public ConsoleSpinner()
        {
            counter = 0;
            sequence = new string[,] {
                { "/", "-", "\\", "|" },
                { ".", "o", "0", "o" },
                { "+", "x","+","x" },
                { "V", "<", "^", ">" },
                { ".   ", "..  ", "... ", "...." },
                { "=>   ", "==>  ", "===> ", "====>" },
            };

            totalSequences = sequence.GetLength(0);
        }

        public void Turn(string displayMsg = "", int sequenceCode = 0)
        {
            counter++;
            Thread.Sleep(Delay);
            sequenceCode = sequenceCode > totalSequences - 1 ? 0 : sequenceCode;
            int counterValue = counter % 4;
            string fullMessage = displayMsg + sequence[sequenceCode, counterValue];
            int msglength = fullMessage.Length;
            Console.Write(fullMessage);
            Console.SetCursorPosition(Console.CursorLeft - msglength, Console.CursorTop);
        }
    }
    public class GameHandle
    {
        public static int currentPlayer = 1;
        public static int col = 50;
        public static int row = 30;
        public static int[,] board = new int[row, col];
        public static int currentPositionX = 5;
        public static int currentPositionY = 5;
        public static int margin = col + 10;
        public static bool isPlaying = false;
        public static void CreateBoard(int col, int row, ref int[,] arr)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if ((i == 0 && j <= col) || (i == row - 1 && j <= col) || (j == 0 && i <= row) || (j == col - 1 && i <= row))
                    {
                        arr[i, j] = 1; // draw the frame of the board
                    }
                    else
                    {
                        arr[i, j] = 0; // space
                    }
                }
            }
        }
        public static void DrawBoard(int col, int row, ref int[,] arr)
        {

            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (arr[i, j] == 1)
                    {
                        System.Console.Write("*");
                    }
                    else if (arr[i, j] == 2)
                    {
                        System.Console.Write("x");
                    }
                    else if (arr[i, j] == 3)
                    {
                        System.Console.Write("o");
                    }
                    else
                    {
                        System.Console.Write(" ");
                    }
                }
                System.Console.WriteLine();
            }
        }
        public static void Goto(ref int x, ref int y)
        {
            Console.SetCursorPosition(margin, 6);
            System.Console.WriteLine("Vi tri hien tai: {0}, {1}", x, y);
            if (x < 1)
            {
                x = 1;
            }
            else if (x > col - 2)
            {
                x = col - 2;
            }
            if (y < 1)
            {
                y = 1;
            }
            else if (y > row - 2)
            {
                y = row - 2;
            }
            Console.SetCursorPosition(x, y);

        }
        public static bool CheckWin(ref int[,] board)
        {

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if ((i == 0 && j <= col) || (i == row - 1 && j <= col) || (j == 0 && i <= row) || (j == col - 1 && i <= row))
                    {
                        // frame
                    }
                    else
                    {
                        if (i > 2 && j > 2)
                        {
                            if (board[i, j] != 0 && board[i - 2, j] != 0 && board[i - 1, j] != 0 && board[i + 1, j] != 0 && board[i + 2, j] != 0)
                            {
                                if (board[i, j] == board[i - 1, j] && board[i, j] == board[i - 2, j] && board[i, j] == board[i + 1, j] && board[i, j] == board[i + 2, j])
                                {
                                    return true;
                                }
                            }
                            else if (board[i, j] != 0 && board[i, j - 2] != 0 && board[i, j - 1] != 0 && board[i, j + 1] != 0 && board[i, j + 2] != 0)
                            {
                                if (board[i, j] == board[i, j - 1] && board[i, j] == board[i, j - 2] && board[i, j] == board[i, j + 1] && board[i, j] == board[i, j + 2])
                                {
                                    return true;
                                }
                            }
                            else if (board[i, j] != 0 && board[i + 1, j - 1] != 0 && board[i + 2, j - 2] != 0 && board[i - 1, j + 1] != 0 && board[i - 2, j + 2] != 0)
                            {
                                if (board[i, j] == board[i + 1, j - 1] && board[i, j] == board[i + 2, j - 2] && board[i, j] == board[i - 1, j + 1] && board[i, j] == board[i - 2, j + 2])
                                {
                                    return true;
                                }
                            }
                            else if (board[i, j] != 0 && board[i - 1, j - 1] != 0 && board[i - 2, j - 2] != 0 && board[i + 1, j + 1] != 0 && board[i + 2, j + 2] != 0)
                            {
                                if (board[i, j] == board[i - 1, j - 1] && board[i, j] == board[i - 2, j - 2] && board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2])
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
        public void Processing()
        {
            isPlaying = true;
            CreateBoard(col, row, ref board);
            Console.Clear();
            DrawGuide();
            DrawBoard(col, row, ref board);
            Direction(ref currentPositionX, ref currentPositionY);
        }
        public static void Reset()
        {
            Console.SetCursorPosition(0, row + 4);
            System.Console.WriteLine("Ban co muon choi lai khong? ");
            System.Console.WriteLine("1. Co");
            System.Console.WriteLine("2. Thoat");

            int choose = 0;
            System.Console.Write("Nhap vao lua chon cua ban: ");
            int.TryParse(Console.ReadLine(), out choose);

            if (choose == 1)
            {
                new GameHandle().Processing();
            }
            else
            {
                Console.Clear();
                System.Console.WriteLine("Thanks for visiting");
                return;
            }
        }
        public static void DrawGuide()
        {
            // change color 
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.Clear();
                if (Convert.ToString(color) == "DarkRed")
                {
                    Console.ForegroundColor = color;
                }
            }
            // title of game
            Console.SetCursorPosition(margin, 0);
            System.Console.WriteLine("------------------------");
            Console.SetCursorPosition(margin, 1);
            System.Console.WriteLine("|       Caro  Game     |");
            Console.SetCursorPosition(margin, 2);
            System.Console.WriteLine("------------------------");
            Console.SetCursorPosition(margin, 4);
            System.Console.WriteLine("Nguoi choi hien tai: " + currentPlayer);
            Console.SetCursorPosition(margin, 6);
            System.Console.WriteLine("Vi tri hien tai: {0}, {1}", currentPositionX, currentPositionY);
            Console.SetCursorPosition(margin, 8);
            System.Console.WriteLine("--------------------------------------");
            Console.SetCursorPosition(margin, 9);
            System.Console.WriteLine("| Nhan phim A de di chuyen sang trai |");
            Console.SetCursorPosition(margin, 10);
            System.Console.WriteLine("| Nhan phim D de di chuyen sang phai |");
            Console.SetCursorPosition(margin, 11);
            System.Console.WriteLine("| Nhan phim S de di chuyen xuong duoi|");
            Console.SetCursorPosition(margin, 12);
            System.Console.WriteLine("| Nhan phim W de di chuyen len tren  |");
            Console.SetCursorPosition(margin, 13);
            System.Console.WriteLine("| Nhan phim Enter de danh            |");
            Console.SetCursorPosition(margin, 14);
            System.Console.WriteLine("| Nhan phim ESC de thoat             |");
            Console.SetCursorPosition(margin, 15);
            System.Console.WriteLine("--------------------------------------");
        }

        public static void Direction(ref int currentPositionX, ref int currentPositionY)
        {
            ConsoleKey key;
            while (isPlaying)
            {
                key = Console.ReadKey(true).Key;
                if (Convert.ToString(key) != "Escape")
                {
                    switch (Convert.ToString(key))
                    {
                        case "W":
                            currentPositionY -= 1;
                            Goto(ref currentPositionX, ref currentPositionY);
                            break;
                        case "D":
                            currentPositionX += 1;
                            Goto(ref currentPositionX, ref currentPositionY);
                            break;
                        case "A":
                            currentPositionX -= 1;
                            Goto(ref currentPositionX, ref currentPositionY);
                            break;
                        case "S":
                            currentPositionY += 1;
                            Goto(ref currentPositionX, ref currentPositionY);
                            break;
                        case "Enter":
                            if (currentPlayer == 1)
                            {
                                if (board[currentPositionY, currentPositionX] != 3)
                                {
                                    board[currentPositionY, currentPositionX] = 2;
                                    currentPlayer = 2;
                                    Console.SetCursorPosition(margin, 4);
                                    System.Console.WriteLine("Nguoi choi hien tai: " + currentPlayer);
                                    DrawBoard(col, row, ref board);
                                    if (CheckWin(ref board))
                                    {
                                        Console.SetCursorPosition(0, row + 2);
                                        System.Console.WriteLine("Nguoi choi " + currentPlayer + "WIN");
                                        isPlaying = false;
                                        Reset();
                                    }
                                }
                            }
                            else
                            {
                                if (board[currentPositionY, currentPositionX] != 2)
                                {
                                    board[currentPositionY, currentPositionX] = 3;
                                    currentPlayer = 1;
                                    Console.SetCursorPosition(margin, 4);
                                    System.Console.WriteLine("Nguoi choi hien tai: " + currentPlayer);
                                    DrawBoard(col, row, ref board);
                                    if (CheckWin(ref board))
                                    {
                                        Console.SetCursorPosition(0, row + 2);
                                        System.Console.WriteLine("Nguoi choi " + currentPlayer + "WIN");
                                        isPlaying = false;
                                        Reset();
                                    }
                                }
                            }
                            Goto(ref currentPositionX, ref currentPositionY);
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    System.Console.WriteLine("Thanks for visiting");
                    return;
                }
            }
        }
    }
    public class Program
    {
        public static void Main()
        {

            Console.Clear();
            ConsoleSpinner spinner = new ConsoleSpinner();
            spinner.Delay = 300;

            while (true)
            {
                foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
                {
                    Console.ForegroundColor = color;
                    spinner.Turn(displayMsg: "Working ", sequenceCode: 5);
                }
                Thread.Sleep(5000);
                break;
            }
            GameHandle game = new GameHandle();
            game.Processing();
        }
    }
}
