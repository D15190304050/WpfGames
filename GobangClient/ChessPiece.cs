namespace GobangClient
{
    /// <summary>
    /// 表示棋盘上某个位置的状态。
    /// </summary>
    public enum ChessPiece
    {
        /// <summary>
        /// 该位置没有棋子。
        /// </summary>
        Blank = 0,

        /// <summary>
        /// 黑棋。
        /// </summary>
        Black = 1,

        /// <summary>
        /// 白棋。
        /// </summary>
        White = 2,
    }
}