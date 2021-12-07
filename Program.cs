using System;


namespace WalkableMaze
{
    public class Program
    {
        static void Main(string[] args)
        {
            Game currentGame = new Game();
            currentGame.Start();
        }
    }




    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        private string PlayerMarker;
        private ConsoleColor PlayerColor;

        public Player(int initialX, int initialY, string playerMarker)
        {
            X = initialX;
            Y = initialY;
            PlayerMarker = playerMarker;
            PlayerColor = ConsoleColor.Red;
        }

        public void Draw()
        {
            Console.ForegroundColor = PlayerColor;
            Console.SetCursorPosition(X, Y);
            Console.Write(PlayerMarker);
            Console.ResetColor();
        }
    }


    class Game
    {
        private World MyWorld;
        private Player CurrentPlayer;
        private Player Godzilla;

        public void Start()
        {
            Console.Title = "Godzilla Rampage";
            Console.CursorVisible = false;


            string[,] grid = {
                   { "=", "=", "=", "=", "=", "=", "=" },
                   { "=", " ", "=", " ", " ", " ", "G" },
                   { " ", " ", "=", " ", "=", " ", "=" },
                   { "=", " ", "=", " ", "=", " ", "=" },
                   { "=", " ", " ", " ", "=", " ", "=" },
                   { "=", "=", "=", "=", "=", "=", "=" },
                };


            MyWorld = new World(grid);

            CurrentPlayer = new Player(0, 2, "M");
            Godzilla = new Player(6, 1, "G");

            RunGameLoop();

        }

        private void DisplayIntro()
        {
            Console.WriteLine("Welcome to Godzilla goes wild in Tokyo");
            Console.WriteLine("Instructions");
            Console.Write("Try to destroy the monster that looks like this: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("G");
            Console.ResetColor();
            Console.WriteLine("> Press any key to start");
            Console.ReadKey(true);

        }

        private void DisplayOutro()
        {
            Console.Clear();
            Console.WriteLine("You destroyed the monster");
            Console.WriteLine("Thanks for saving the city");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }
        private void DrawFrame()
        {
            Console.Clear();
            MyWorld.Draw();
            CurrentPlayer.Draw();
            Godzilla.Draw();
        }

        private void HandlePlayerInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X, CurrentPlayer.Y - 1))
                    {
                        CurrentPlayer.Y -= 1;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X, CurrentPlayer.Y + 1))
                    {
                        CurrentPlayer.Y += 1;
                    }

                    break;
                case ConsoleKey.LeftArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X - 1, CurrentPlayer.Y))
                    {
                        CurrentPlayer.X -= 1;
                    }

                    break;
                case ConsoleKey.RightArrow:
                    if (MyWorld.IsPositionWalkable(CurrentPlayer.X + 1, CurrentPlayer.Y))
                    {
                        CurrentPlayer.X += 1;
                    }

                    break;
                default:
                    break;

            }
        }

        private void RunGameLoop()
        {
            DisplayIntro();
            while (true)
            {
                //Draw everything
                DrawFrame();

                //Check for player input
                HandlePlayerInput();

                //Check if the player has reached the exit and end game if so
                string elementAtPlayerPos = MyWorld.GetElementAt(CurrentPlayer.X, CurrentPlayer.Y);
                if (elementAtPlayerPos == "G")
                {
                    break;
                }
                //Give the console a chance to render
                System.Threading.Thread.Sleep(40);
                //break;
            }
            DisplayOutro();
        }

    }

    class World
    {
        private string[,] Grid;
        private int Rows;
        private int Cols;

        public World(string[,] grid)
        {
            Grid = grid;
            Rows = Grid.GetLength(0);
            Cols = Grid.GetLength(1);
        }

        public void Draw()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    string element = Grid[y, x];
                    Console.SetCursorPosition(x, y);

                    if (element == "G")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(element);
                }

            }
        }

        public string GetElementAt(int x, int y)
        {
            return Grid[y, x];
        }

        public bool IsPositionWalkable(int x, int y)
        {
            //Check bounds
            if (x < 0 || y < 0 || x >= Cols || y >= Rows)
            {
                return false;
            }

            //Check if grid is walkable
            return Grid[y, x] == " " || Grid[y, x] == "G";
        }

    }
}