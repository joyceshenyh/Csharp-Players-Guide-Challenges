public interface ICommand
{
    public bool RunCommand(Player player, GameMap map);
}

public class ActivateFountainCommand : ICommand
{
    public bool RunCommand(Player player, GameMap map)
    {
        if (map.CheckCurrentRoom(0, map.MapDimension / 2) == RoomCondition.FountainOff && (player.X, player.Y) == (0, map.MapDimension / 2))
        {
            map.ActivateFountain();
            return true;
        }
        else if (map.CheckCurrentRoom(0, map.MapDimension / 2) == RoomCondition.FountainOn)
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
        if (player.X >= 0 && player.X <= (map.MapDimension - 1) &&
            player.Y + 1 >= 0 && player.Y + 1 <= (map.MapDimension - 1))
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
        if (player.X >= 0 && player.X <= (map.MapDimension - 1) &&
            player.Y - 1 >= 0 && player.Y - 1 <= (map.MapDimension - 1))
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
        if (player.X + 1 >= 0 && player.X + 1 <= (map.MapDimension - 1) &&
            player.Y >= 0 && player.Y <= (map.MapDimension - 1))
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
        if (player.X - 1 >= 0 && player.X - 1 <= (map.MapDimension - 1) &&
            player.Y >= 0 && player.Y <= (map.MapDimension - 1))
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