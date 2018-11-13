using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// The <see cref="MinMaxPruning" /> class represents the min-max-pruning algorithm for the "Tic-Tac-Toe" game AI to search for the best position for its next step.
    /// </summary>
    public class MinMaxPruning
    {
        /// <summary>
        /// Calculate the best position for game AI to move according to current chessboard state.
        /// </summary>
        /// <param name="currentState">Current state of the chessboard.</param>
        /// <param name="playerMark">Chess piece mark of the player to move.</param>
        /// <param name="rowIndex">Row index of the best position to move calculated by Min-Max-Pruning.</param>
        /// <param name="columnIndex">Column index of the best position to move calculated by Min-Max-Pruning.</param>
        /// <remarks>
        /// The "player" here doesn't mean the human player of this game, it only means either AI or the player of this game.
        /// </remarks>
        public static void GetBestPosition(Chessboard currentState, ChessPiece playerMark, out int rowIndex, out int columnIndex)
        {
            // "out" parameter must be assigned.
            rowIndex = 0;
            columnIndex = 0;
            
            // Get the opponent's mark.
            ChessPiece opponentMark = playerMark == ChessPiece.X ? ChessPiece.O : ChessPiece.X;

            // Initialize the "max" to int.MinValue.
            int max = int.MinValue;

            // Iterate through every blank cell for current player.
            for (int playerRowIndex = 0; playerRowIndex < Chessboard.Size; playerRowIndex++)
            {
                for (int playerColumnIndex = 0; playerColumnIndex < Chessboard.Size; playerColumnIndex++)
                {
                    if (currentState[playerRowIndex, playerColumnIndex] == ChessPiece.Blank)
                    {
                        // Make a deep copy of current state and put the player mark in the blank cell of the deep copy.
                        Chessboard nextStep = currentState.Copy();
                        nextStep.AddChessPiece(playerRowIndex, playerColumnIndex, playerMark);

                        // Initialize the "min" to int.MaxValue.
                        int min = int.MaxValue;

                        // Iterate through every blank cell for current player's opponent.
                        for (int opponentRowIndex = 0; opponentRowIndex < Chessboard.Size; opponentRowIndex++)
                        {
                            for (int opponentColumnIndex = 0; opponentColumnIndex < Chessboard.Size; opponentColumnIndex++)
                            {
                                if (nextStep[opponentRowIndex, opponentColumnIndex] == ChessPiece.Blank)
                                {
                                    // Make a deep copy of current state and put the opponent's mark in the blank cell of the deep copy.
                                    Chessboard nextNextStep = nextStep.Copy();
                                    nextNextStep.AddChessPiece(opponentRowIndex, opponentColumnIndex, opponentMark);

                                    // Evaluate the state according to the player score and opponent score.
                                    int stateScore = PlayerScore(nextNextStep, playerMark) - OpponentScore(nextNextStep, opponentMark);

                                    // Keep tracking the min value.
                                    if (stateScore < min)
                                        min = stateScore;
                                }
                            }
                        }

                        // If min > max, then current (playerRowIndex, playerColumnIndex) is better than any other cell searched before.
                        if (min > max)
                        {
                            max = min;
                            rowIndex = playerRowIndex;
                            columnIndex = playerColumnIndex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates how many lines can the player win according to the simulated chessboard state.
        /// </summary>
        /// <param name="state">The simulated chessboard state.</param>
        /// <param name="playerMark">Chess piece mark of the player to move.</param>
        /// <returns>The number of lines for the player to win.</returns>
        /// <remarks>
        /// Although this method has only 2 lines, this is implemented for the sake of readability.
        /// </remarks>
        private static int PlayerScore(Chessboard state, ChessPiece playerMark)
        {
            Chessboard filledChessboard = FillChessboard(state, playerMark);
            return CalculateScore(filledChessboard, playerMark);
        }

        /// <summary>
        /// Calculates how many lines can the opponent win according to the simulated chessboard state.
        /// </summary>
        /// <param name="state">The simulated chessboard state.</param>
        /// <param name="opponentMark">Chess piece mark of the opponent to move.</param>
        /// <returns>The number of lines for the opponent to win.</returns>
        /// <remarks>
        /// Although this method has only 2 lines, this is implemented for the sake of readability.
        /// </remarks>
        private static int OpponentScore(Chessboard state, ChessPiece opponentMark)
        {
            Chessboard filledChessboard = FillChessboard(state, opponentMark);
            return CalculateScore(filledChessboard, opponentMark);
        }

        /// <summary>
        /// Returns a deep copy of the input <see cref="Chessboard" /> will all blank cells are filled with the specified mark.
        /// </summary>
        /// <param name="chessboard">The input (original) <see cref="Chessboard" />.</param>
        /// <param name="mark">The chess piece mark to fill the blank cells.</param>
        /// <returns>A deep copy of the input <see cref="Chessboard" /> will all blank cells are filled with the specified mark.</returns>
        private static Chessboard FillChessboard(Chessboard chessboard, ChessPiece mark)
        {
            Chessboard filledChessboard = chessboard.Copy();
            for (int i = 0; i < Chessboard.Size; i++)
            {
                for (int j = 0; j < Chessboard.Size; j++)
                {
                    if (filledChessboard[i, j] == ChessPiece.Blank)
                        filledChessboard.AddChessPiece(i, j, mark);
                }
            }

            return filledChessboard;
        }

        /// <summary>
        /// Calculates how many lines can the player using the given mark win according to the simulated chessboard state.
        /// </summary>
        /// <param name="filledChessboard">An instance of <see cref="Chessboard" /> with no blank cells.</param>
        /// <param name="mark">The chess piece mark of the player.</param>
        /// <returns>The number of lines for the player to win.</returns>
        /// <remarks>
        /// The "player" here doesn't mean the human player of this game, it only means either AI or the player of this game.
        /// </remarks>
        private static int CalculateScore(Chessboard filledChessboard, ChessPiece mark)
        {
            // Initialize the score to 0, increase it every time a possible line is found.
            int score = 0;

            // A boolean mark indicating whether the player wins.
            bool add;

            // Score of rows.
            for (int i = 0; i < Chessboard.Size; i++)
            {
                add = true;
                for (int j = 0; j < Chessboard.Size; j++)
                {
                    if (filledChessboard[i, j] != mark)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                    score++;
            }

            // Score of columns.
            for (int j = 0; j < Chessboard.Size; j++)
            {
                add = true;
                for (int i = 0; i < Chessboard.Size; i++)
                {
                    if (filledChessboard[i, j] != mark)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                    score++;
            }

            // Main diagonal.
            add = true;
            for (int i = 0; i < Chessboard.Size; i++)
            {
                if (filledChessboard[i, i] != mark)
                {
                    add = false;
                    break;
                }
            }
            if (add)
                score++;

            // Vice diagonal.
            add = true;
            for (int i = 0; i < Chessboard.Size; i++)
            {
                if (filledChessboard[i, Chessboard.Size - 1 - i] != mark)
                {
                    add = false;
                    break;
                }
            }
            if (add)
                score++;

            return score;
        }
    }
}
