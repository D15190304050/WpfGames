using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// The <see cref="Chessboard" /> class represents a 3x3 chessboard of the "Tic-Tac-Toe" game.
    /// </summary>
    public class Chessboard
    {
        /// <summary>
        /// Size of the chessboard, which equals the number of rows and the number of columns of the chessboard.
        /// </summary>
        public const int Size = 3;

        /// <summary>
        /// The background chessboard.
        /// </summary>
        private readonly ChessPiece[,] chessboard;

        /// <summary>
        /// Gets a value indicating whether this chessboard is full of non-blank chess pieces.
        /// </summary>
        public bool IsFull
        {
            get
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (this[i, j] == ChessPiece.Blank)
                            return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Ges the chess piece of the cell with specified row index and column index.
        /// </summary>
        /// <param name="rowIndex">Row index of the specified cell.</param>
        /// <param name="columnIndex">Column index of the specified cell.</param>
        /// <returns>The chess piece of the cell with specified row index and column index.</returns>
        public ChessPiece this[int rowIndex, int columnIndex]
        {
            get { return chessboard[rowIndex, columnIndex]; }
            private set { chessboard[rowIndex, columnIndex] = value; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Chessboard" /> with all cells blank.
        /// </summary>
        public Chessboard()
        {
            chessboard = new ChessPiece[Chessboard.Size, Chessboard.Size];
            for (int i = 0; i < Chessboard.Size; i++)
            {
                for (int j = 0; j < Chessboard.Size; j++)
                    chessboard[i, j] = ChessPiece.Blank;
            }
        }

        /// <summary>
        /// Adds a chess piece at the specified cell of this chessboard.
        /// </summary>
        /// <param name="rowIndex">Row index of the cell to add the new chess piece.</param>
        /// <param name="columnIndex">Column index of the cell to add the new chess piece.</param>
        /// <param name="chessPiece">The chess piece to add.</param>
        /// <returns>True if the chess piece is added successfully (i.e. the cell was blank), otherwise, false.</returns>
        public bool AddChessPiece(int rowIndex, int columnIndex, ChessPiece chessPiece)
        {
            if (this[rowIndex, columnIndex] == ChessPiece.Blank)
            {
                this[rowIndex, columnIndex] = chessPiece;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears this <see cref="Chessboard" />.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    this[i, j] = ChessPiece.Blank;
            }
        }

        /// <summary>
        /// Returns a deep copy of this <see cref="Chessboard" /> with the same state.
        /// </summary>
        /// <returns>A deep copy of this <see cref="Chessboard" /> with the same state.</returns>
        public Chessboard Copy()
        {
            Chessboard copy = new Chessboard();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    copy[i, j] = this[i, j];
            }

            return copy;
        }
    }
}
