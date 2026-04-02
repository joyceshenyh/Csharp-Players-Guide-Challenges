public class GameMap
{
    private RoomCondition[,] rooms;
    private static Random rand = new Random();
    public int MapDimension { get; }

    public GameMap(int dimension)
    {
        MapDimension = dimension;
        rooms = new RoomCondition[dimension, dimension];

        for (int i = 0; i < dimension; i++)
        {
            for (int a = 0; a < dimension; a++)
            {
                rooms[i, a] = RoomCondition.Empty;
            }
        }

        //rooms represented by an 2D array of RoomCondition s
        //in the start of the build set all rooms to empty/default

        BuildSpecialRoom(RoomCondition.Pit, dimension / 2);

        //then build pit rooms, 2 for small, 3 for medium, 4 for large
                
        if(dimension == 8)
        {
            BuildSpecialRoom(RoomCondition.Maelstrom, 2);
        }
        else
        {
            BuildSpecialRoom(RoomCondition.Maelstrom, 1);
        }
        
        //then build Maelstrom room(s), 1 for small and medium, 2 for large

        rooms[0, dimension / 2] = RoomCondition.FountainOff;
        rooms[0, 0] = RoomCondition.Entrance;

        //finally build entrance and fountain rooms
    }

    private void BuildSpecialRoom(RoomCondition typeOfSpecialRoom, int quantity)
    {
        int specialX = 0;
        int specialY = 0;
        bool isValidSpecialRoom = false;

        for (int count = 0; count < quantity; count++)
        {
            while (!isValidSpecialRoom)
            {
                specialX = rand.Next(MapDimension);
                specialY = rand.Next(MapDimension);
                isValidSpecialRoom = (specialX, specialY) != (0, 0) && (specialX, specialY) != (0, MapDimension / 2) &&
                rooms[specialX, specialY] == RoomCondition.Empty;
            }
            rooms[specialX, specialY] = typeOfSpecialRoom;
            isValidSpecialRoom = false;
        }
    }

    //builds special rooms of choice
    //ensures the special room doesn't take the place of the entrance, fountain room, or another special room

    public RoomCondition CheckCurrentRoom(int X, int Y) => rooms[X, Y];

    private bool HasSomethingNearby(RoomCondition typeOfSpecialRoom, int X, int Y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                int XChecked = X + dx;
                int YChecked = Y + dy;

                bool isInMap = (XChecked >= 0 && XChecked <= MapDimension - 1) && (YChecked >= 0 && YChecked <= MapDimension - 1);
                if (isInMap)
                {
                    if (rooms[XChecked, YChecked] == typeOfSpecialRoom)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //checks for special rooms around the current position
    //only checks those rooms within range of the map (i.e. ignores the walls)

    public bool HasPitNearby(int X, int Y) => HasSomethingNearby(RoomCondition.Pit, X, Y);
    public bool HasMaelstromNearby(int X, int Y) => HasSomethingNearby(RoomCondition.Maelstrom, X, Y);
    
    public void ActivateFountain() => rooms[0, MapDimension/2] = RoomCondition.FountainOn;

    public void MoveMaelstrom(int X, int Y)
    {
        rooms[X, Y] = RoomCondition.Empty;
        BuildSpecialRoom(RoomCondition.Maelstrom, 1);

        //this actually doesn't follow the writer's exact instructions
        //i.e. the Maelstrom always move one space south and two spaces west w/ wrapping
        //I made it random move
    }

    public void GettingShot((int X, int Y) roomBeingShot)
    {
        if(roomBeingShot == (-1, -1))
        {
            return;

            //if the user did not enter a valid shot, ignore this action
        }

        else if (roomBeingShot.X > MapDimension - 1 || roomBeingShot.X < 0 || roomBeingShot.Y > MapDimension - 1 || roomBeingShot.Y < 0)
        {
            Console.WriteLine("You shot into the boundary wall. You hear a thud and nothing happened.");
            return;
        }

        if (rooms[roomBeingShot.X, roomBeingShot.Y] == RoomCondition.Maelstrom)
        {
            Console.WriteLine("You shot right into the heart of the maelstrom! It has now been cleared.");
            rooms[roomBeingShot.X, roomBeingShot.Y] = RoomCondition.Empty;
        }
        else
        {
            Console.WriteLine("The arrow fly into the room you point it at. But nothing seem to have happened.");
        }
    }

    public static GameMap AskForMapSize()
    {        
        int mapDimension = 0;
        while(mapDimension == 0)
        {
            Console.WriteLine("Choose your game map size.");
            Console.WriteLine("Small: 4*4\nMedium: 6*6\nLarge: 8*8");
            string userSize = Console.ReadLine().ToLower();
            mapDimension = userSize switch
            {
                "small" => 4,
                "medium" => 6,
                "large" => 8,
                _ => 0
            };
        }
        return new GameMap(mapDimension);
    }

}
