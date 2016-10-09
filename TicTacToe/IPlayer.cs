namespace TicTacToe
{
    internal interface IPlayer
    {
        void Play(bool?[] board, bool playerSymbol);
    }
}