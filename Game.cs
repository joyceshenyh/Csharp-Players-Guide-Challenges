public class Game
{
    private GameMap map;
    private Player player;
    private DateTime beginTime;
    private DateTime endTime;

    public Game()
    {
        map = GameMap.AskForMapSize();
        player = new Player();
        beginTime = DateTime.Now;
        Console.Clear();
    }

    private void DisplayHelp()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nEnter 'move north', 'move south', 'move east', or 'move west' to move in the corresponding direction," +
            "as long as you are not hitting the boundary walls.");
        Console.WriteLine("Enter 'activate fountain' to activate the Fountain of Objects if you are in the fountain room, or does nothing if you are not.");
        Console.WriteLine("You carry with you a bow and a quiver of arrows. " +
            "\nYou can use them to shoot monsters in the caverns but be warned: you have a limited supply.\n");
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    private void DisplayTimePlayed()
    {
        TimeSpan timeInGame = endTime - beginTime;
        Console.WriteLine($"\nYou have played for {timeInGame.Hours} hours, {timeInGame.Minutes} minutes, and {timeInGame.Seconds} seconds." +
            $"\nThank you for playing!");
    }

    public void Play()
    {
        Console.WriteLine("-------------------------------------------------------------------------------------");
        Console.WriteLine("\nYou enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects." +
                "\nLight is visible only in the entrance, and no other light is seen anywhere in the caverns." +
                "\nYou must navigate the Caverns with your other senses." +
                "\nFind the Fountain of Objects, activate it, and return to the entrance.");
        Console.WriteLine("\nLook out for pits. You will feel a breeze if a pit is in an adjacent room. If you enter a room with a pit, you will die.");
        Console.WriteLine("\nMaelstroms are violent forces of sentient wind. " +
                          "Entering a room with one could transport you to another location in the caverns." +
                          "You will be able to hear their growling and groaning in nearby rooms.\n");

        while (true)
        {
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine($"You are currently in the room at Row={player.X}, Column={player.Y}.");
            Console.WriteLine($"You currently have {player.NumOfArrows} arrows left.\n");
            RoomCondition currentRoomStatus = map.CheckCurrentRoom(player.X, player.Y);
            string message = currentRoomStatus switch
            {
                RoomCondition.Entrance => "You see light in this room coming from outside the cavern. This is the entrance.",
                RoomCondition.FountainOff => "You hear water dripping in this room. The Fountain of Objects is here!",
                RoomCondition.FountainOn => "You hear the rushing waters from the Fountain of Objects. It has been reactivated!",
                RoomCondition.Pit => "Oh no - This is a pit room! You fell into it and failed.",
                RoomCondition.Maelstrom => "You have been blown by the maelstrom!",
                RoomCondition.Empty => "You hear your steps against the marble floor, nothing seems to be inside this room.",
            };

            //notify the player of current room's condition
            Console.WriteLine(message);        

            if (currentRoomStatus == RoomCondition.Maelstrom)
            {
                int maelstromX = player.X;
                int maelstromY = player.Y;
                player.GetBlownByMaelstrom(map);
                map.MoveMaelstrom(maelstromX, maelstromY);
                continue;

                //if the current room has the Maelstrom, the player gets blown and this round ends
            }

            //notify the player of nearby rooms' conditions
            if (map.HasPitNearby(player.X, player.Y))
            {
                Console.WriteLine("You feel a draft. There is a pit in a nearby room.");
            }
            if(map.HasMaelstromNearby(player.X, player.Y))
            {
                Console.WriteLine("You hear the growling and groaning of a maelstrom nearby.");
            }

            //evaluate the game outcome and decide if the game continues                       
            if (currentRoomStatus == RoomCondition.Pit)
            {
                Console.WriteLine($"Better luck next trial!");
                endTime = DateTime.Now;
                DisplayTimePlayed();
                return;

                //player fails if they steps into a pit room, game ends
            }

            else if (currentRoomStatus == RoomCondition.Entrance && map.CheckCurrentRoom(0, map.MapDimension/2) == RoomCondition.FountainOn)
            {
                Console.WriteLine("The Fountain of Objects has been reactivated, and you have escaped with your life!\nYou win!");
                endTime = DateTime.Now;
                DisplayTimePlayed();
                return;

                //if the player return to entrance with fountain activated they win, game ends
            }

            else
            {
                map.GettingShot(player.ShootArrow());

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

                        if(playerAction == "help")
                        {
                            DisplayHelp();
                            continue;
                        }

                        if(nextRoundCommand == null)
                        {
                            Console.WriteLine("Sorry, the fountain does not understand that. Please try again.");
                        }

                        //only advance to executing the command if it is one of the valid options
                    }

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