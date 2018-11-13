using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Image path of the circle mark.
        /// </summary>
        private const string CircleImagePath = "Images/o.png";

        /// <summary>
        /// Image path of the cross mark.
        /// </summary>
        private const string CrossImagePath = "Images/x.png";

        /// <summary>
        /// Image path of the transparent image.
        /// </summary>
        private const string TransparentImagePath = "Images/Transparent.bmp";

        /// <summary>
        /// The player's mark is set to "O".
        /// </summary>
        private const ChessPiece PlayerMark = ChessPiece.O;

        /// <summary>
        /// The AI's mark is set to "X".
        /// </summary>
        private const ChessPiece AiMark = ChessPiece.X;

        /// <summary>
        /// The image of player's mark.
        /// </summary>
        private readonly BitmapImage PlayerMarkImage = new BitmapImage(new Uri(CircleImagePath, UriKind.Relative));

        /// <summary>
        /// The image of AI's mark.
        /// </summary>
        private readonly BitmapImage AiMarkImage = new BitmapImage(new Uri(CrossImagePath, UriKind.Relative));

        /// <summary>
        /// The transparent image.
        /// </summary>
        private readonly BitmapImage TransparentImage = new BitmapImage(new Uri(TransparentImagePath, UriKind.Relative));

        /// <summary>
        /// The background chessboard of the "Tic-Tac-Toe" game.
        /// </summary>
        private readonly Chessboard chessboard;

        /// <summary>
        /// The corresponding 2-D array of buttons, each of which represents a UI cell of the chessboard.
        /// </summary>
        private Button[,] chessPositions;

        /// <summary>
        /// A boolean value indicating whether the game is running.
        /// </summary>
        private bool gameStarted;

        /// <summary>
        /// Initializes the game window and internal data structures.
        /// </summary>
        public MainWindow()
        {
            chessboard = new Chessboard();
            gameStarted = false;

            InitializeComponent();
        }

        /// <summary>
        /// Handles the player move event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_PlayerMove(object sender, RoutedEventArgs e)
        {
            // Do nothing if the game is not started or no one can move any more.
            if (!gameStarted || !CanMove())
                return;

            // Get the clicked button and extract the corresponding coordinate.
            Button cmd = (Button) e.OriginalSource;
            string content = cmd.Tag.ToString();
            string[] position = content.Split(',');
            int playerRowIndex = int.Parse(position[0]);
            int playerColumnIndex = int.Parse(position[1]);

            // Try to add a chess piece at the player specified position.
            if (chessboard.AddChessPiece(playerRowIndex, playerColumnIndex, PlayerMark))
            {
                // Update the UI of the button clicked by the player.
                Image playerMarkImage = new Image();
                playerMarkImage.Source = PlayerMarkImage;
                cmd.Content = playerMarkImage;

                // Check whether the player wins.
                if (PlayerWin())
                {
                    MessageBox.Show("YOU WIN!!!");
                    return;
                }

                // AI moves if it can move.
                if (CanMove())
                {
                    AiMove();

                    // Check whether AI wins.
                    if (AiWin())
                        MessageBox.Show("AI WIN!!!");
                }
            }
        }

        /// <summary>
        /// Starts a new game once the player click this button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStartGame_Click(object sender, RoutedEventArgs e)
        {
            // Start game if the player specifies who moves firstly.
            if ((rbAiFirst.IsChecked != true) && (rbPlayerFirst.IsChecked != true))
            {
                MessageBox.Show("Please specify who is the first to move");
                return;
            }

            // Start a new game by clearing the chessboard.
            ClearChessboard();
            gameStarted = true;

            // Let AI move if it should move firstly.
            if (rbAiFirst.IsChecked == true)
                AiMove();
        }

        /// <summary>
        /// Gets the references of the buttons corresponding to each chess position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <reamrks>
        /// This method must be called after this window is loaded, since the references of buttons is available only after this window is loaded.
        /// An exception will occured if you call the method before the window is loaded.
        /// </reamrks>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chessPositions = new Button[Chessboard.Size, Chessboard.Size];
            chessPositions[0, 0] = chessPosition00;
            chessPositions[0, 1] = chessPosition01;
            chessPositions[0, 2] = chessPosition02;
            chessPositions[1, 0] = chessPosition10;
            chessPositions[1, 1] = chessPosition11;
            chessPositions[1, 2] = chessPosition12;
            chessPositions[2, 0] = chessPosition20;
            chessPositions[2, 1] = chessPosition21;
            chessPositions[2, 2] = chessPosition22;
        }

        /// <summary>
        /// Clear the chessboard.
        /// </summary>
        private void ClearChessboard()
        {
            // Clear the background chessboard.
            chessboard.Clear();

            // Clear the chessboard UI.
            for (int i = 0; i < Chessboard.Size; i++)
            {
                for (int j = 0; j < Chessboard.Size; j++)
                {
                    Image transparentImage = new Image();
                    transparentImage.Source = TransparentImage;
                    chessPositions[i, j].Content = transparentImage;
                }
            }
        }

        /// <summary>
        /// AI moves a step.
        /// </summary>
        private void AiMove()
        {
            // Get the best position for AI to move next by the Min Max Pruning algorithm.
            MinMaxPruning.GetBestPosition(chessboard, AiMark, out int aiRowIndex, out int aiColumnIndex);

            // Add a chess piece at the calculated position.
            chessboard.AddChessPiece(aiRowIndex, aiColumnIndex, AiMark);
            Image aiMarkImage = new Image();
            aiMarkImage.Source = AiMarkImage;
            chessPositions[aiRowIndex, aiColumnIndex].Content = aiMarkImage;
        }

        /// <summary>
        /// Gets a value indicating whether AI or the player can still move.
        /// </summary>
        /// <returns>True if AI or the player can still move, otherwise, false.</returns>
        /// <remarks>
        /// Although this method has only 1 line, this is implemented for the sake of readability.
        /// </remarks>
        private bool CanMove()
        {
            return !(chessboard.IsFull || PlayerWin() || AiWin());
        }

        /// <summary>
        /// Gets a value indicating whether the player is win.
        /// </summary>
        /// <returns>True if the player is win, otherwise, false.</returns>
        /// <remarks>
        /// Although this method has only 1 line, this is implemented for the sake of readability.
        /// </remarks>
        private bool PlayerWin()
        {
            return Win(PlayerMark);
        }

        /// <summary>
        /// Gets a value indicating whether AI is win.
        /// </summary>
        /// <returns>True if AI is win, otherwise, false.</returns>
        /// <remarks>
        /// Although this method has only 1 line, this is implemented for the sake of readability.
        /// </remarks>
        private bool AiWin()
        {
            return Win(AiMark);
        }

        /// <summary>
        /// Gets a value indicating whether the player with the given mark wins.
        /// </summary>
        /// <param name="mark">The mark of the player.</param>
        /// <returns>True if the player with the given mark wins, otherwise, false.</returns>
        /// <remarks>
        /// The "player" here doesn't mean the human player of this game, it only means either AI or the player of this game.
        /// </remarks>
        private bool Win(ChessPiece mark)
        {
            // A boolean mark indicating whether the user wins.
            bool win;

            // Check rows.
            for (int i = 0; i < Chessboard.Size; i++)
            {
                win = true;
                for (int j = 0; j < Chessboard.Size; j++)
                {
                    if (chessboard[i, j] != mark)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return true;
            }

            // Check columns.
            for (int j = 0; j < Chessboard.Size; j++)
            {
                win = true;
                for (int i = 0; i < Chessboard.Size; i++)
                {
                    if (chessboard[i, j] != mark)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return true;
            }

            // Check main diagonal.
            win = true;
            for (int i = 0; i < Chessboard.Size; i++)
            {
                if (chessboard[i, i] != mark)
                {
                    win = false;
                    break;
                }
            }

            if (win)
                return true;

            // Check vice diagonal.
            win = true;
            for (int i = 0; i < Chessboard.Size; i++)
            {
                if (chessboard[i, Chessboard.Size - 1 - i] != mark)
                {
                    win = false;
                    break;
                }
            }

            if (win)
                return true;

            return false;
        }
    }
}
