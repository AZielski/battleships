namespace Battleships
{
    public static class BattleshipsMain
    {
        private static int BoardSize => 10;
        private static char[,] Board { get; } = new char[BoardSize, BoardSize];
        private static Ship[] Ships { get; } = {
            new ("Battleship", 5, new Random().Next(0,100) <= 50 ? Positioning.Horizontal : Positioning.Vertical),
            new ("Destroyer", 4, new Random().Next(0,100) <= 50 ? Positioning.Horizontal : Positioning.Vertical),
            new ("Destroyer", 4, new Random().Next(0,100) <= 50 ? Positioning.Horizontal : Positioning.Vertical)
        };
        
        
        public static void Main()
        {
            FillBoard();
            GenerateShips();
            DrawBoard();
            
            while (Ships.Any(x => x.IsAlive))
            {
                HandleInput();
                DrawBoard();
            }
            
            Console.WriteLine("You won! Congratulations!");
        }

        private static void HandleInput()
        {
            Console.WriteLine("Enter coordinates to strike (ex. A1)");
            var input = Console.ReadLine();

            if (input is null)
            {
                Console.WriteLine("Please enter coordinates (ex. A1)");
                return;
            }

            if (!char.IsLetter(input[0]) || !int.TryParse(input[1..], out var number))
            {
                Console.WriteLine("Please enter coordinates in example format [Letter|Number](ex. A1)");
                return;
            }

            if (number is < 1 or > 10)
            {
                Console.WriteLine("Please enter coordinates in provided range A-J 1-10");
                return;
            }

            var cords = new Tuple<int, int>(number, input[0] - '@');
            var hitShip = Ships.FirstOrDefault(x => x.Coordinates.Contains(cords));

            if (hitShip is null)
            {
                Console.WriteLine("You've missed");
                Board[cords.Item1 - 1, cords.Item2 - 1] = 'U';
                return;
            }
            
            hitShip.Hit();
            Board[cords.Item1 - 1, cords.Item2 - 1] = '&';
        }

        private static void GenerateShips()
        {
            var rand = new Random();
            
            foreach (var ship in Ships)
            {
                ship.Coordinates = new Tuple<int, int>[ship.Length];
                int x = 0, y = 0;
                var check = false;

                while (!check)
                {
                    x = rand.Next(2,8);
                    y = rand.Next(2,8);
                    
                    for (var i = 0; i < ship.Length; i++)
                    {
                        check = ship.Positioning switch
                        {
                            Positioning.Horizontal => Board[y, x - 2 + i] != 'x',
                            Positioning.Vertical => Board[y - 2 + i, x] != 'x',
                            _ => check
                        };

                        if (!check)
                        {
                            break;
                        }
                    }
                }

                for (var i = 0; i < ship.Length; i++)
                {
                    if (ship.Positioning == Positioning.Horizontal)
                    {
                        ship.Coordinates[i] = new Tuple<int, int>(y + 1, x - 1 + i);
                        continue;
                    }

                    ship.Coordinates[i] = new Tuple<int, int>(y - 1 + i, x + 1);
                }
            }
        }
        
        private static void FillBoard()
        {
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    Board[i,j] = 'o';
                }
            }
        }

        private static void DrawBoard()
        {
            Console.WriteLine("\tA B C D E F G H I J");
            
            for (var i = 0; i < BoardSize; i++)
            {
                Console.Write($"{i+1}\t");
                
                for (var j = 0; j < BoardSize; j++)
                {
                    Console.Write(Board[i,j] + " ");
                }
    
                Console.Write("\n");
            }
        }
    }
}