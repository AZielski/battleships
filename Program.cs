namespace Battleships
{
    public static class BattleshipsMain
    {
        private const int BoardSize = 10;
        private static char[,] Board { get; } = new char[BoardSize, BoardSize];
        private static Ship[] Ships { get; } = {
            new ("Battleship", 5),
            new ("Destroyer", 4),
            new ("Destroyer", 4),
        };
        
        public static void Main()
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
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

            if (number is < 1 or > BoardSize)
            {
                Console.WriteLine("Please enter coordinates in provided range A-J 1-10");
                return;
            }

            //parse input to coordinates
            var letter = input[0];
            var column = char.IsUpper(letter) ? letter - 'A' : letter - 'a';
            var row = number - 1;
            
            var cords = new Tuple<int, int>(row, column);
            var hitShip = Ships.FirstOrDefault(x => x.Coordinates.Contains(cords));

            if (hitShip is null)
            {
                Console.WriteLine("You've missed");
                Board[cords.Item1, cords.Item2] = 'U';
                return;
            }
            
            hitShip.Hit();
            Board[cords.Item1, cords.Item2] = '&';
        }

        private static void GenerateShips()
        {
            var rand = new Random();
            
            foreach (var ship in Ships)
            {
                ship.Coordinates = new Tuple<int, int>[ship.Length];
                int x = 0, y = 0;
                var check = false;
                var direction = rand.Next(0, 100) < 50 ? Positioning.Horizontal : Positioning.Vertical;
                
                while (!check)
                {
                    //randomly generate coordinates
                    x = rand.Next(2, BoardSize - 2);
                    y = rand.Next(2, BoardSize - 2);
                    
                    for (var i = 0; i < ship.Length; i++)
                    {
                        check = direction switch
                        {
                            Positioning.Horizontal => Board[y, x - 2 + i] != 'x',
                            Positioning.Vertical => Board[y - 2 + i, x] != 'x',
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        if (!check)
                        {
                            break;
                        }
                    }
                }

                //Place ship on board
                for (var i = 0; i < ship.Length; i++)
                {
                    ship.Coordinates[i] = direction switch
                    {
                        Positioning.Horizontal => new Tuple<int, int>(y, x - 2 + i),
                        Positioning.Vertical => new Tuple<int, int>(y - 2 + i, x),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            }
        }
        
        //Fill board with empty cells
        private static void FillBoard()
        {
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    Board[i, j] = 'o';
                }
            }
        }

        //Draw battleships board
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