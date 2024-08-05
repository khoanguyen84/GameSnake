namespace GameSnake;

class Program
{
    static bool gameOver = false;
    static string direction = "RIGHT";
    static readonly string BRICK_SYMBOL = "#";
    static readonly string SPACE_SYMBOL = " ";
    static readonly string SNAKE_SYMBOL = "*";
    static readonly string FOOD_SYMBOL = "@";
    static bool isExistFood = false;
    static int food_x = -1;
    static int food_y = -1;
    static int score = 0;
    static int speed = 500;
    static readonly int rows = 20;
    static int cols = 40;
    static string[,] board = new string[rows, cols];
    static int[] headSnake = new int[2] { 1, 1 };
    static int[][] bodySnake = new int[2][]
    {
        new int[2] {-1, -1},
        new int[2] {-1, -1}
    };
    static DateTime startTime = DateTime.Now;
    static DateTime endTime;
    static int durationTime = 0;
    static string directory = @"d:/";
    static void Main(string[] args)
    {
        Thread gameThread = new Thread(ListenKey);
        gameThread.Start();

        while (!gameOver)
        {
            Console.Clear();
            ResetBoard();
            DrawSnake();
            RandomFood();
            DrawBoard();
            SnakeMoving(direction);
            Task.Delay(speed).Wait();
        }
        if (gameOver)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game over");
            Console.ResetColor();
            Environment.Exit(0);
        }

        // FileWriter(Path.Combine(directory, "history.txt"), FileMode.Append, "Khoa");
        // FileWriter(Path.Combine(directory, "log.txt"), FileMode.Create, "Nguyễn");
    }

    // static void FileWriter(string path, FileMode mode, string data)
    // {
    //     FileStream fs = new FileStream(path, mode);
    //     using (StreamWriter sw = new StreamWriter(fs))
    //     {
    //         sw.WriteLine(data);
    //     }
    // }

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
                else if (x == food_x && y == food_y)
                {
                    board[x, y] = FOOD_SYMBOL;
                }
                else
                {
                    board[x, y] = SPACE_SYMBOL;
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
                    // Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(value);
                    Console.ResetColor();
                }
                else if (value.Equals(SNAKE_SYMBOL))
                {
                    if (x == headSnake[0] && y == headSnake[1])
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(value);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(value);
                        Console.ResetColor();
                    }
                }
                else if (value.Equals(FOOD_SYMBOL))
                {
                    Console.ForegroundColor = ConsoleColor.White;
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
        Console.WriteLine($"Score: {score}");
        ShowTimer();
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
        //Check Game is Over
        CheckGameIsOver();

        int head_snake_x;
        int head_snake_y;
        switch (direction)
        {
            case "RIGHT":
                head_snake_y = headSnake[1];
                headSnake[1] = head_snake_y + 1;
                if (headSnake[1] == cols - 1)
                {
                    headSnake[1] = 1;
                }
                BodySnakeMoving(headSnake[0], head_snake_y);
                break;
            case "LEFT":
                head_snake_y = headSnake[1];
                headSnake[1] = head_snake_y - 1;
                if (headSnake[1] == 0)
                {
                    headSnake[1] = cols - 2;
                }
                BodySnakeMoving(headSnake[0], head_snake_y);
                break;
            case "DOWN":
                head_snake_x = headSnake[0];
                headSnake[0] = head_snake_x + 1;
                if (headSnake[0] == rows - 1)
                {
                    headSnake[0] = 1;
                }
                BodySnakeMoving(head_snake_x, headSnake[1]);
                break;
            case "UP":
                head_snake_x = headSnake[0];
                headSnake[0] = head_snake_x - 1;
                if (headSnake[0] == 0)
                {
                    headSnake[0] = rows - 2;
                }
                BodySnakeMoving(head_snake_x, headSnake[1]);
                break;
            default:
                break;
        }
        //Eat Food
        EatFood();
    }

    static void BodySnakeMoving(int head_snake_x, int head_snake_y)
    {
        for (int b = 0; b < bodySnake.Length; b++)
        {
            int tmp_body_x = bodySnake[b][0];
            int tmp_body_y = bodySnake[b][1];
            bodySnake[b][0] = head_snake_x;
            bodySnake[b][1] = head_snake_y;
            head_snake_x = tmp_body_x;
            head_snake_y = tmp_body_y;
        }
    }

    static void ListenKey()
    {
        while (!gameOver)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
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

    static void RandomFood()
    {
        if (!isExistFood)
        {
            do
            {
                Random random = new();
                food_x = random.Next(1, rows - 2);
                food_y = random.Next(1, cols - 2);
                isExistFood = true;
            }
            while (FoodInSnake());
        }
    }

    static bool FoodInSnake()
    {
        foreach (int[] body in bodySnake)
        {
            if (body[0] == food_x && body[1] == food_y)
            {
                return true;
            }
        }
        return headSnake[0] == food_x && headSnake[1] == food_y;
    }

    static void EatFood()
    {
        if (headSnake[0] == food_x && headSnake[1] == food_y)
        {
            score++;
            speed = speed > 10 ? speed - 10 : 0;
            Array.Resize(ref bodySnake, bodySnake.Length + 1);
            bodySnake[bodySnake.Length - 1] = new int[] { -1, -1 };
            isExistFood = false;
        }
    }

    static void CheckGameIsOver()
    {
        foreach (int[] body in bodySnake)
        {
            if (body[0] == headSnake[0] && body[1] == headSnake[1])
            {
                gameOver = true;
            }
        }
    }

    static void ShowTimer()
    {
        endTime = DateTime.Now;
        durationTime = Convert.ToInt32(Math.Round((endTime - startTime).TotalSeconds, 0));
        Console.WriteLine($"{TimeSpan.FromSeconds(durationTime)}");
    }
}
