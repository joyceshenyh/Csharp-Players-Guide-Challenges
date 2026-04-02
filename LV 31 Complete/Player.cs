public class Player
{
    public int X { get; set; }
    public int Y { get; set; }

    public int NumOfArrows { get; private set; }

    public Player()
    {
        X = 0;
        Y = 0;
        NumOfArrows = 5;
    }

    public (int, int) ShootArrow()
    {
        (int, int) roomToShootInto = (-1, -1);
        if(NumOfArrows > 0)
        {
            Console.Write("\nYou may choose to shoot an arrow in any of the four following directions: " +
                "north, south, east, west." +
                "\nEnter 'shoot north' to shoot to the corresponding direction." +
                "\nEnter anything else to quit shooting in this round. ");

            string userAction = Console.ReadLine().ToLower();
            roomToShootInto = userAction switch
            {
                "shoot north" => (X, Y + 1),
                "shoot south" => (X, Y - 1),
                "shoot east" => (X + 1, Y),
                "shoot west" => (X - 1, Y),
                _ => (-1, -1),
            };

            if(roomToShootInto != (-1, -1))
            {
                NumOfArrows--;
            }
        }           

        else
        {
            Console.WriteLine("Sorry, you don't have any arrows left. You cannot shoot anymore.");
        }

        return roomToShootInto;
    }

    //player has X and Y properties to record their current location
    //works together with GameMap's current room

    public void GetBlownByMaelstrom(GameMap map)
    {
        X = X < map.MapDimension - 2 ? X + 2 : map.MapDimension - 1;
        Y = Y < map.MapDimension - 1 ? Y + 1 : map.MapDimension - 1;

        //the player moves one space north and two spaces east
        //if they are against the boundary they are just moved to the boundary
    }
}
