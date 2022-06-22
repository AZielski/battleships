namespace Battleships;

public class Ship
{
    private string Name { get; init; }
    public Tuple<int, int>[] Coordinates { get; set; }
    public int Length { get; init; }
    public bool IsAlive { get; private set; } = true;
    private int HitCount { get; set; }

    public Ship(string name, int length)
    {
        Name = name;
        Length = length;
        Coordinates = new Tuple<int, int>[length];
    }
    
    public void Hit()
    {
        Console.WriteLine($"You've hit a ship of name: {Name}");
        HitCount += 1;

        if (HitCount != Length)
        {
            return;
        }
        
        IsAlive = false;
        Console.WriteLine($"You've sank a ship of name: {Name}");
    }
}

public enum Positioning
{
    Horizontal,
    Vertical
}