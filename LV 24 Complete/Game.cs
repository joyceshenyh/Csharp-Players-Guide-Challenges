namespace TicTacToeGame;
public class Game
{
    public GameBoard GameBoard { get; }
    public int[] GameStats { get; }

    public Game()
    {
        GameBoard = new GameBoard();
        GameStats = new int[] { 0, 0, 0 };
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
        (int totalRound, int winRequiredRound) gameSetup = ChooseGameMode();
        //Get the game mode

        int roundCount = 1;
        for (int count = 0; count < gameSetup.totalRound; count++)
        {
            GameBoard.ClearBoard();
            Console.WriteLine($"\nGame Stats: Player 1: {GameStats[1]} Player 2: {GameStats[2]}," +
                          $"Draw: {GameStats[0]}");
            Console.WriteLine($"Round: {roundCount}/{gameSetup.Item1}");

            PlayARound();
            roundCount++;

            if (GameStats[1] == gameSetup.winRequiredRound || GameStats[2] == gameSetup.winRequiredRound)
            {
                int winningPlayer = GameStats[1] == gameSetup.winRequiredRound ? 1 : 2;
                Console.WriteLine($"The overall winner is player {winningPlayer}! Congratulations!");
                return;
            }
            //whenever someone reached #winning rounds needed, the game ends
        }
    }

    private (int, int) ChooseGameMode()
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