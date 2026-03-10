//Tic-Tac-Toe
//A nice birthday accomplishment i guess? Happy 20 to myself!

Game myGame = new Game();
myGame.PlayMultipleRounds();

class TicTacToeBoard
{
    public char[] Board { get; private set; }

    public TicTacToeBoard()
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

class Game
{
    public TicTacToeBoard GameBoard { get; }
    public int[] GameStats { get; }

    public Game()
    {
        GameBoard = new TicTacToeBoard();
        GameStats = new int[] {0, 0, 0};
    }

    public void PlayARound()
    {
        int gameCurrentState = -1;
        int whoseTurn = 0;
        while (gameCurrentState == -1)
        {
            GameBoard.ChooseSpace(whoseTurn % 2 + 1);
            gameCurrentState = GameBoard.EvaluateWinner();
            whoseTurn++;
        }
        //while no line has formed and there are still empty spot, game continues
        //'whoseTurn' here marks which player's turn it is

        Console.Clear();
        GameBoard.DisplayBoard();
        GameStats[gameCurrentState] += 1;
        //records the result of this round

        string message = gameCurrentState switch
        {
            1 => "Player 1 has won!",
            2 => "Player 2 has won!",
            0 => "It's a draw!"
        };
        Console.WriteLine(message);

        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
        //haven't learnt how to wait a few second
        //this acts as a pause for user to read the messages before next round
    }

    public void PlayMultipleRounds()
    {
        (int, int) gameSetup = ChooseGameMode();
        //Get the game mode

        int roundCount = 1;
        for (int count = 0; count < gameSetup.Item1; count++){
            GameBoard.ClearBoard();
            Console.WriteLine($"\nGame Stats: Player 1: {GameStats[1]} Player 2: {GameStats[2]}," +
                          $"Draw: {GameStats[0]}");
            Console.WriteLine($"Round: {roundCount}/{gameSetup.Item1}");

            PlayARound();
            roundCount++;

            if (GameStats[1] == gameSetup.Item2 || GameStats[2] == gameSetup.Item2)
            {
                int winningPlayer = GameStats[1] == gameSetup.Item2 ? 1 : 2;
                Console.WriteLine($"The overall winner is player {winningPlayer}! Congratulations!");
                return;
            }
            //whenever someone reached #winning rounds needed, the game ends
        }
    }

    public (int, int) ChooseGameMode()
    {
        int choice = 0;
        do
        {
            Console.WriteLine($"Which game mode do you prefer?");
            Console.WriteLine($"1 - One round decides the winner" +
                            $"\n2 - Two out of three rounds" +
                            $"\n3 - Three out of five rounds");

            choice = Convert.ToInt32(Console.ReadLine());
        }
        while (!(choice == 1 || choice == 2 || choice == 3));
        //only proceed if choice is one of the available options
        
        Console.Clear();

        return choice switch
        {
            1 => (1, 1),
            2 => (3, 2),
            3 => (5, 3),
        };
        //(#total rounds to play, #winning rounds needed for overall win)
    }
}