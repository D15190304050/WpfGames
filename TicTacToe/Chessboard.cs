using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Chessboard
    {
        public const int Size = 3;
        private readonly ChessPiece[,] chessboard;

        public ChessPiece this[int rowIndex, int columnIndex]
        {
            get { return chessboard[rowIndex, columnIndex]; }
            private set { chessboard[rowIndex, columnIndex] = value; }
        }

        public Chessboard()
        {
            chessboard = new ChessPiece[Chessboard.Size, Chessboard.Size];
            for (int i = 0; i < Chessboard.Size; i++)
            {
                for (int j = 0; j < Chessboard.Size; j++)
                    chessboard[i, j] = ChessPiece.Blank;
            }
        }

        public bool AddChessPiece(int rowIndex, int columnIndex, ChessPiece chessPiece)
        {
            if (this[rowIndex, columnIndex] != ChessPiece.Blank)
            {
                this[rowIndex, columnIndex] = chessPiece;
                return true;
            }
            return false;
        }
    }
}
