using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using static System.Console;
namespace Змейка
{
    internal class Program
    {
        private const int Delay = 200;

        private const int MapWidth = 20;
        private const int MapHeight = 20;

        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeight = MapHeight * 3;

        private const ConsoleColor BorderColor = ConsoleColor.Gray;
        private const ConsoleColor HeadColor = ConsoleColor.Green;
        private const ConsoleColor BodyColor = ConsoleColor.DarkGreen;
        private const ConsoleColor FoodColor = ConsoleColor.Red;

        private static readonly Random random = new Random();
        static void Main()  
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible= false;
            while (true)
            {
                StarGame();
                Thread.Sleep(100);
                ReadKey();
            }
        }

        static void StarGame()
        {
            Clear();
            DrawBorder();

            Directions currentMovement = Directions.Right;

            var snake = new Snake(10, 5, HeadColor, BodyColor);
            Pixel food = GenFood(snake);
            food.Draw();
            Stopwatch stopwatch = new Stopwatch();

            while (true)
            {
                stopwatch.Restart();
                Directions oldMovemnt = currentMovement;

                while (stopwatch.ElapsedMilliseconds <= Delay)
                {
                    if (currentMovement == oldMovemnt)
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }
                }

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake);
                    food.Draw();
                }
                else
                {
                    snake.Move(currentMovement);
                }
                if (snake.Head.X == MapWidth - 1 || snake.Head.X == 0 || snake.Head.Y == MapHeight - 1 || snake.Head.Y == 0 || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;
            }

            snake.Clear();
            SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
            WriteLine("Вы проиграли");
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;
            do
            {
                food = new Pixel(random.Next(1, MapWidth - 2), random.Next(1, MapHeight - 2), FoodColor);
            }while (snake.Head.X == food.X && snake.Head.Y == food.Y || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));
            return food;
        }

        static Directions ReadMovement(Directions currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;
            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Directions.Down => Directions.Up,
                ConsoleKey.DownArrow when currentDirection != Directions.Up => Directions.Down,
                ConsoleKey.LeftArrow when currentDirection != Directions.Right => Directions.Left,
                ConsoleKey.RightArrow when currentDirection != Directions.Left => Directions.Right,
                _ => currentDirection
            };
            return currentDirection;
        }
        static void DrawBorder()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor,1).Draw();
                new Pixel(i, MapHeight - 1, BorderColor, 1).Draw();
            }
            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor,1).Draw();
                new Pixel(MapWidth - 1, i, BorderColor, 1).Draw();
            }
        }
    }
}