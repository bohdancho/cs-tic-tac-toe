Game _ = new();

class Game
{
    private readonly Board board = new();

    public Game()
    {
        while (true)
        {
            Console.WriteLine(board);

            TakeAndApplyPlayerMove();
            if (CheckWinnerAndPrint()) break;
            GetAndApplyComputerMove();
            if (CheckWinnerAndPrint()) break;
        }
    }

    private bool CheckWinnerAndPrint()
    {
        char? winner = board.GetWinner();
        if (winner is not null)
        {
            Console.WriteLine(board);

            string message = winner switch
            {
                'X' => "You won over the machine. The apocalypse is averted",
                'O' => "You lost to a machine. LMAO!",
                _ => throw new Exception()
            };

            Console.WriteLine(message);
            return true;
        }
        return false;
    }

    private void TakeAndApplyPlayerMove()
    {
        while (true)
        {
            try
            {
                (int playerMoveX, int playerMoveY) = TakePlayerMove();
                board.MakeMove('X', playerMoveX, playerMoveY);
                Console.WriteLine();
                break;
            }
            catch (FieldTakenException)
            {
                Console.WriteLine("This field is already taken!");
                Console.WriteLine();
            }
        }
    }

    private static (int, int) TakePlayerMove()
    {
        Console.WriteLine("Next move?");
        string? input = Console.ReadLine();

        string invalidMoveMessage = "Invalid move. Syntax: \"YX\", where Y is the column and X is the row (0..2) \n";

        if (input is null || input.Length != 2)
        {
            Console.WriteLine(invalidMoveMessage);
            return TakePlayerMove();
        }

        int x = (int)char.GetNumericValue(input[0]);
        int y = (int)char.GetNumericValue(input[1]);

        if (x < 0 || x > 2 || y < 0 || y > 2)
        {
            Console.WriteLine(invalidMoveMessage);
            return TakePlayerMove();
        }

        return (x, y);
    }

    private void GetAndApplyComputerMove()
    {
        (int computerMoveX, int computerMoveY) = GetComputerMove();
        board.MakeMove('O', computerMoveX, computerMoveY);
        Console.WriteLine();
    }

    private (int, int) GetComputerMove()
    {
        Random rnd = new();

        int x; int y;
        do
        {
            x = rnd.Next(0, 3); y = rnd.Next(0, 3);
        } while (!board.IsFieldFree(x, y));

        return (x, y);
    }

}

class Board
{

    private readonly char[,] fields = { { '-', '-', '-', }, { '-', '-', '-', }, { '-', '-', '-', }, };

    public override string ToString()
    {
        string str = "";
        for (int y = 0; y < fields.GetLength(0); y++)
        {
            for (int x = 0; x < fields.GetLength(1); x++)
            {
                str += fields[y, x] + " ";
            }
            str += '\n';
        }
        return str;
    }

    public void MakeMove(char player, int x, int y)
    {
        if (!IsFieldFree(x, y))
        {
            throw new FieldTakenException();
        }

        fields[x, y] = player;
    }

    public bool IsFieldFree(int x, int y)
    {
        return fields[x, y] == '-';
    }

    public char? GetWinner()
    {
        if (DidPlayerWin('X')) return 'X';
        if (DidPlayerWin('O')) return 'O';
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
                if (fields[x, y] != player) won = false;
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

public class FieldTakenException : Exception
{
    public FieldTakenException()
    {
    }

    public FieldTakenException(string? message) : base(message)
    {
    }

    public FieldTakenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}

