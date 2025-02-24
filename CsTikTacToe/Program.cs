Game _ = new();

class Game
{
    public Game()
    {
        while (true)
        {
            PrintBoard();
            Console.WriteLine();

            (int playerMoveX, int playerMoveY) = TakePlayerMove();
            board[playerMoveX, playerMoveY] = 'X';

            (int computerMoveX, int computerMoveY) = GetComputerMove();
            board[computerMoveX, computerMoveY] = 'Y';
            Console.WriteLine();

            char? winner = GetWinner();
            if (winner is not null)
            {
                PrintBoard();
                Console.WriteLine();
                Console.WriteLine(winner + " won!");
                break;
            }
        }
    }

    private readonly char[,] board = { { '-', '-', '-', }, { '-', '-', '-', }, { '-', '-', '-', }, };

    private void PrintBoard()
    {
        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(1); x++)
            {
                Console.Write(board[y, x] + " ");
            }
            Console.Write('\n');
        }
    }

    private (int, int) TakePlayerMove()
    {
        Console.WriteLine("Next move?");
        string? input = Console.ReadLine();

        if (input is null)
        {
            Console.WriteLine("Invalid move. Try again:");
            return TakePlayerMove();
        }

        int x = (int)char.GetNumericValue(input[0]);
        int y = (int)char.GetNumericValue(input[1]);

        if (board[x, y] != '-')
        {
            Console.WriteLine("This field is taken. Try again:");
            return TakePlayerMove();
        }

        return (x, y);
    }

    private (int, int) GetComputerMove()
    {
        Random rnd = new();

        int x; int y;
        do
        {
            x = rnd.Next(0, 3); y = rnd.Next(0, 3);
        } while (board[x, y] != '-');

        return (x, y);
    }

    private char? GetWinner()
    {
        if (DidPlayerWin('X')) return 'X';
        if (DidPlayerWin('Y')) return 'Y';
        return null;
    }

    private bool DidPlayerWin(char player)
    {
        for (int i = 0; i < winningMasks.GetLength(0); i++)
        {
            bool won = true;
            for (int j = 0; j < winningMasks.GetLength(1); j++)
            {
                (int x, int y) = winningMasks[i, j];
                if (board[x, y] != player) won = false;
            }
            if (won) return true;
        }
        return false;
    }

    private static readonly Tuple<int, int>[,] winningMasks = {
        {Tuple.Create(0,0),Tuple.Create(0,1),Tuple.Create(0,2)},
        {Tuple.Create(1,0),Tuple.Create(1,1),Tuple.Create(1,2)},
        {Tuple.Create(2,0),Tuple.Create(2,1),Tuple.Create(2,2)},
        {Tuple.Create(0,0),Tuple.Create(1,0),Tuple.Create(2,0)},
        {Tuple.Create(0,1),Tuple.Create(1,1),Tuple.Create(2,1)},
        {Tuple.Create(0,2),Tuple.Create(1,2),Tuple.Create(2,2)},
        {Tuple.Create(0,0),Tuple.Create(1,1),Tuple.Create(2,2)},
        {Tuple.Create(0,2),Tuple.Create(1,1),Tuple.Create(2,0)}
    };
}
