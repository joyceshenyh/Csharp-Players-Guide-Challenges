//The Fountain Of Objects
Game myGame = new Game();
myGame.Play();

public enum RoomCondition { Empty, Entrance, FountainOff, FountainOn, Trap }

public interface ICommand
{
    public bool RunCommand(Player player, GameMap map);
}

public class ActivateFountainCommand : ICommand
{
    public bool RunCommand(Player player, GameMap map)
    {
        if(map.CheckCurrentRoom(0, 2) == RoomCondition.FountainOff && (player.X, player.Y) == (0, 2))
        {
            map.ActivateFountain();
            return true;
        }
        else if (map.CheckCurrentRoom(0, 2) == RoomCondition.FountainOn)
        {
            Console.WriteLine("The fountain is already activated! You may return.");
            return false;
        }
        else
        {
            Console.WriteLine("You are not in the fountain room. You cannot activate it here.");
            return false;
        }

        //check if player is in the fountain room & the fountain is off
        //if conditions are not met, the command cannot be executed
    }
}

public class MoveNorthCommand : ICommand
{
    public bool RunCommand(Player player, GameMap map)
    {
        if (player.X >= 0 && player.X <= 3 &&
            player.Y + 1 >= 0 && player.Y + 1 <= 3)
        {
            player.Y += 1;
            return true;
        }
        else
        {
            Console.WriteLine("You have reached the edge of the map. Try a different direction.");
            return false;
        }

        //check if the player can make this move (not hitting the wall)
        //same with the other move commands
    }
}

public class MoveSouthCommand : ICommand
{
    public bool RunCommand(Player player, GameMap map)
    {
        if (player.X >= 0 && player.X <= 3 &&
            player.Y - 1 >= 0 && player.Y - 1 <= 3)
        {
            player.Y -= 1;
            return true;
        }
        else
        {
            Console.WriteLine("You have reached the edge of the map. Try a different direction.");
            return false;
        }
    }
}

public class MoveEastCommand : ICommand
{
    public bool RunCommand(Player player, GameMap map)
    {
        if (player.X + 1 >= 0 && player.X + 1 <= 3 &&
            player.Y >= 0 && player.Y <= 3)
        {
            player.X += 1;
            return true;
        }
        else
        {
            Console.WriteLine("You have reached the edge of the map. Try a different direction.");
            return false;
        }
    }
}

public class MoveWestCommand : ICommand
{
    public bool RunCommand(Player player, GameMap map)
    {
        if (player.X - 1 >= 0 && player.X - 1 <= 3 &&
            player.Y >= 0 && player.Y <= 3)
        {
            player.X -= 1;
            return true;
        }
        else
        {
            Console.WriteLine("You have reached the edge of the map. Try a different direction.");
            return false;
        }
    }
}

public class GameMap
{
    private RoomCondition[,] rooms;
    private static Random rand = new Random();

    public GameMap()
    {
        rooms = new RoomCondition[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int a = 0; a < 4; a++)
            {
                rooms[i, a] = RoomCondition.Empty;
            }
        }
        for (int count = 0; count < 3; count++)
        {
            rooms[rand.Next(4), rand.Next(4)] = RoomCondition.Trap;
        }
        rooms[0, 2] = RoomCondition.FountainOff;
        rooms[0, 0] = RoomCondition.Entrance;

        //2D array of RoomCondition s
        //in the start of each game build random trap rooms first
        //then build entrance and fountain (to ensure they dont get replaced by traps)
    }

    public RoomCondition CheckCurrentRoom(int X, int Y) => rooms[X, Y];

    public void ActivateFountain() => rooms[0, 2] = RoomCondition.FountainOn;

}

public class Player
{
    public int X { get; set; }
    public int Y { get; set; }

    public Player()
    {
        X = 0;
        Y = 0;
    }

    //player has X and Y properties to record their current location
    //works together with GameMap's current room
}

public class Game
{
    private GameMap map;
    private Player player;

    public Game()
    {
        map = new GameMap();
        player = new Player();
    }

    public void Play()
    {
        while (true)
        {
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine($"You are currently in the room at Row={player.X}, Column={player.Y}.");
            RoomCondition currentRoomStatus = map.CheckCurrentRoom(player.X, player.Y);
            string message = currentRoomStatus switch
            {
                RoomCondition.Entrance => "You see light in this room coming from outside the cavern. This is the entrance.",
                RoomCondition.FountainOff => "You hear water dripping in this room. The Fountain of Objects is here!",
                RoomCondition.FountainOn => "You hear the rushing waters from the Fountain of Objects. It has been reactivated!",
                RoomCondition.Trap => "Oh no - This is a trap room! You failed.",
                RoomCondition.Empty => "You hear your steps against the marble floor, nothing seems to be inside this room.",
            };
            Console.WriteLine(message);
            //check current room status in the start of each round and decide if game ends or the player continue

            if (currentRoomStatus == RoomCondition.Trap)
            {
                Console.WriteLine($"Better luck next trial!");
                return;

                //player fails if they steps into a trap room, game ends
            }

            else if (currentRoomStatus == RoomCondition.Entrance && map.CheckCurrentRoom(0, 2) == RoomCondition.FountainOn)
            {
                Console.WriteLine("The Fountain of Objects has been reactivated, and you have escaped with your life!\nYou win!");
                return;

                //if the player return to entrance with fountain activated they win, game ends
            }

            else
            {
                string? playerAction;
                ICommand? nextRoundCommand = null;
                bool isValidCommand = false;

                while (!isValidCommand)
                {
                    while (nextRoundCommand == null)
                    {
                        Console.Write("What do you want to do? ");
                        playerAction = Console.ReadLine().ToLower();
                        nextRoundCommand = playerAction switch
                        {
                            "move east" => new MoveEastCommand(),
                            "move west" => new MoveWestCommand(),
                            "move north" => new MoveNorthCommand(),
                            "move south" => new MoveSouthCommand(),
                            "activate fountain" => new ActivateFountainCommand(),
                            _ => null,
                        };
                    }
                    //only advance to executing the command if it is one of the valid options

                    isValidCommand = nextRoundCommand.RunCommand(player, map);
                    if (!isValidCommand)
                    {
                        nextRoundCommand = null;

                        //if current command cannot be executed, clear the command memory and ask the player to try another
                    }
                }

            }

        }

    }

}