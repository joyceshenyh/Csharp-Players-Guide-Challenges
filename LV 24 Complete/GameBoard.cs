namespace TicTacToeGame;
public class GameBoard
{
    public char[] Board { get; private set; }

    public GameBoard()
    {
        Board = new char[9];
        for (int count = 0; count < 9; count++)
        {
            Board[count] = ' ';
        }
    }
    //create new board w/ 9 empty spaces

    public void ChooseSpace(int whichPlayer)
    {
        Console.Clear();
        DisplayBoard();
        //Clears console before printing the updated board to feel more real

        Console.WriteLine($"Choose a square you want to play in, player {whichPlayer}.");
        int whichSpace;
        do
        {
            whichSpace = Convert.ToInt32(Console.ReadLine());
            if (whichSpace > Board.Length || whichSpace <= 0)
            {
                Console.WriteLine("The board does not contain this space. Try again.");
            }
            else if (Board[whichSpace - 1] != ' ')
            {
                Console.WriteLine("This space has already been occupied. Try again.");
            }
        }
        while (!(whichSpace <= Board.Length && whichSpace > 0 && Board[whichSpace - 1] == ' '));
        //Only allow user to choose a space within range and is currently empty

        Board[whichSpace - 1] = whichPlayer switch //Note: arrays starts w/ index 0
        {
            1 => 'X',
            2 => 'O'
        };
        //Fill in the space based on player # (Assume this is a two-player game always)
    }

    public void DisplayBoard()
    {
        Console.WriteLine($" {Board[0]} | {Board[1]} | {Board[2]} ");
        Console.WriteLine($"---+---+---");
        Console.WriteLine($" {Board[3]} | {Board[4]} | {Board[5]} ");
        Console.WriteLine($"---+---+---");
        Console.WriteLine($" {Board[6]} | {Board[7]} | {Board[8]} ");
    }

    public int EvaluateWinner()
    {
        int[][] winConditions = new int[][] { new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8},
                                              new int[] {0, 3, 6}, new int[] {1, 4, 7}, new int[] {2, 5, 8},
                                              new int[] {0, 4, 8}, new int[] {2, 4, 6} };
        char winner = ' ';

        foreach (int[] condition in winConditions)
        {
            if ((Board[condition[0]] == 'X' || Board[condition[0]] == 'O')
                && Board[condition[1]] == Board[condition[0]]
                && Board[condition[2]] == Board[condition[1]])
            {
                winner = Board[condition[0]];
                return winner switch
                {
                    'X' => 1,
                    'O' => 2
                };
            }
        }
        //if someone made a line they win

        foreach (char ch in Board)
        {
            if (ch == ' ')
            {
                return -1;
            }
        }

        return 0;

        //No line is made; if there are still empty space, the game in continuing; if not, it's a draw

    }

    public void ClearBoard()
    {
        for (int count = 0; count < 9; count++)
        {
            Board[count] = ' ';
        }
    }

}
