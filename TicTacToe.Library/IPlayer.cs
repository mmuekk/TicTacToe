namespace TicTacToe.Library
{
    public interface IPlayer
    {
        int Play(Board board);

        ESymbol Symbol { get; set; }
    }
}