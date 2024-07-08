namespace GameSnake;

class Program
{
    static bool gameOver = false;
    static string direction = "RIGHT";
    static readonly string BRICK_SYMBOL = "#";
    static readonly string SPACE_SYMBOL = " ";
    static readonly string SNAKE_SYMBOL = "*";
    static readonly int speed = 500;
    static readonly int rows = 15;
    static int cols = 30;
    static string[,] board = new string[rows, cols];
    static int[] headSnake = new int[2] { 1, 1 };
    static int[][] bodySnake = new int[2][]
    {
        new int[2] {-1, -1},
        new int[2] {-1, -1}
    };
    static void Main(string[] args)
    {
        Thread gameThread = new Thread(ListenKey);
        gameThread.Start();
        while (true)
        {
            Console.Clear();
            ResetBoard();
            DrawSnake();
            DrawBoard();
            SnakeMoving(direction);
            Task.Delay(speed).Wait();
        }
    }

    static void ResetBoard()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                if (x == 0 || x == rows - 1 || y == 0 || y == cols - 1)
                {
                    board[x, y] = BRICK_SYMBOL;
                }
                else
                {
                    board[x, y] = SNAKE_SYMBOL;
                }
            }
        }
    }

    static void DrawBoard()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                string value = board[x, y];
                if (value.Equals(BRICK_SYMBOL))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(value);
                    Console.ResetColor();
                }
                else if (value.Equals(SNAKE_SYMBOL))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(value);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(value);
                }
            }
            Console.WriteLine();
        }
    }

    static void DrawSnake()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                int snake_head_x = headSnake[0];
                int snake_head_y = headSnake[1];
                if (x == snake_head_x && y == snake_head_y)
                {
                    board[x, y] = SNAKE_SYMBOL;
                }
                for (int b = 0; b < bodySnake.Length; b++)
                {
                    if (x == bodySnake[b][0] && y == bodySnake[b][1])
                    {
                        board[x, y] = SNAKE_SYMBOL;
                    }
                }
            }
        }
    }
    static void SnakeMoving(string direction)
    {
        int current_x;
        int current_y;
        switch (direction)
        {
            case "RIGHT":
                current_y = headSnake[1];
                headSnake[1] = current_y + 1;
                if (headSnake[1] == cols - 1)
                {
                    headSnake[1] = 1;
                }
                BodySnakeMoving(headSnake[0], current_y);
                break;
            case "LEFT":
                current_y = headSnake[1];
                headSnake[1] = current_y - 1;
                if (headSnake[1] == 0)
                {
                    headSnake[1] = cols - 2;
                }
                BodySnakeMoving(headSnake[0], current_y);
                break;
            case "DOWN":
                current_x = headSnake[0];
                headSnake[0] = current_x + 1;
                if (headSnake[0] == rows - 1)
                {
                    headSnake[0] = 1;
                }
                BodySnakeMoving(current_x, headSnake[1]);
                break;
            case "UP":
                current_x = headSnake[0];
                headSnake[0] = current_x - 1;
                if (headSnake[0] == 0)
                {
                    headSnake[0] = rows - 2;
                }
                BodySnakeMoving(current_x, headSnake[1]);
                break;
            default:
                break;

        }
    }

    static void BodySnakeMoving(int body_x, int body_y)
    {
        for (int b = 0; b < bodySnake.Length; b++)
        {
            int tmp_body_x = bodySnake[b][0];
            int tmp_body_y = bodySnake[b][1];
            bodySnake[b][0] = body_x;
            bodySnake[b][1] = body_y;
            body_x = tmp_body_x;
            body_y = tmp_body_y;
        }
    }
    static void ListenKey()
    {
        while (!gameOver)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine(direction);
            switch (keyInfo.Key)
            {
                case ConsoleKey.RightArrow:
                    if (direction != "LEFT")
                    {
                        direction = "RIGHT";
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (direction != "RIGHT")
                    {
                        direction = "LEFT";
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != "UP")
                    {
                        direction = "DOWN";
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (direction != "DOWN")
                    {
                        direction = "UP";
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
