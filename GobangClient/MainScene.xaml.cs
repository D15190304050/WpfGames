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
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Threading;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for MainScene.xaml
    /// </summary>
    public partial class MainScene : Window
    {
        /// <summary>
        /// By convention, Gobang is a game played on 15*15 chessboard by two players.
        /// </summary>
        private const int ChessboardSize = 15;
        private const int LineSize = 5;

        private const string ChessPieceStyle = "ChessPieceStyle";
        private const string BlackChessPieceStyle = "BlackChessPieceStyle";
        private const string WhiteChessPieceStyle = "WhiteChessPieceStyle";

        private ChessPiece[,] chessboard;
        private Button[,] chessPieceButtons;
        private LinkedList<Ellipse> chessPieceCircles;
        private string localAccount;
        private bool matchStarted;
        private bool canPutChessPiece;
        private string opponentAccount;
        private ChessPiece color;

        public JToken MatchInfo { get; set; }

        public SearchForMatchWindow SearchForMatchWindow { get; set; }

        public MainScene(string localAccount)
        {
            chessPieceButtons = new Button[ChessboardSize, ChessboardSize];
            chessPieceCircles = new LinkedList<Ellipse>();
            this.localAccount = localAccount;
            chessboard = new ChessPiece[ChessboardSize, ChessboardSize];

            InitializeComponent();
        }

        // Draw grid lines once the window is loaded.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int numGridLines = ChessboardSize - 1;
            double chessboardColumnWidth = chessboardCanvas.ActualWidth / numGridLines;
            double chessboardRowHeight = chessboardCanvas.ActualHeight / numGridLines;

            DrawChessboardGridLines(numGridLines, chessboardColumnWidth, chessboardRowHeight);
        }

        // Clear all user movements.
        private void Restart()
        {
            // Clear placed chess pieces, which is stored in chessPieceCircles.
            foreach (Ellipse e in chessPieceCircles)
                chessboardCanvas.Children.Remove(e);

            // Clear the chess piece collection.
            chessPieceCircles.Clear();

            // Clear message area.
            lstChatting.Items.Clear();
            txtMessageToSend.Clear();

            // Clear match information.
            txtLocalAccount.Text = "";
            txtOpponentAccount.Text = "";
            localAccountChessPiece.Fill = Brushes.Transparent;
            opponentAccountChessPiece.Fill = Brushes.Transparent;

            // Remove chess piece buttons and reset the chessboard.
            for (int i = 0; i < ChessboardSize; i++)
            {
                for (int j = 0; j < ChessboardSize; j++)
                {
                    chessboardCanvas.Children.Remove(chessPieceButtons[i, j]);
                    chessPieceButtons[i, j] = null;
                    chessboard[i, j] = ChessPiece.Blank;
                }
            }

            // Reset match state.
            matchStarted = false;
            canPutChessPiece = false;

            // Draw chess piece buttons.
            int numGridLines = ChessboardSize - 1;
            double chessboardColumnWidth = chessboardCanvas.ActualWidth / numGridLines;
            double chessboardRowHeight = chessboardCanvas.ActualHeight / numGridLines;
            InitializeChessPieceButtons(numGridLines, chessboardColumnWidth, chessboardRowHeight);
        }

        public new void Show()
        {
            base.Show();
            Restart();

            // Negotiate the order once both players entered the game.
            if (localAccount == MatchInfo[JsonPackageKeys.InitiatorAccount].ToString())
                new OrderNegotiationWindow(localAccount, MatchInfo).Show();
        }

        // Try data binding to make grid lines change adaptively.
        private void DrawChessboardGridLines(int numGridLines, double chessboardColumnWidth, double chessboardRowHeight)
        {
            // Draw row lines.
            for (int i = 1; i < numGridLines; i++)
            {
                Line rowLine = new Line();
                rowLine.X1 = 0;
                rowLine.Y1 = chessboardRowHeight * i;
                rowLine.X2 = chessboardCanvas.ActualWidth;
                rowLine.Y2 = chessboardRowHeight * i;
                rowLine.StrokeThickness = 1;
                rowLine.Stroke = Brushes.Black;
                chessboardCanvas.Children.Add(rowLine);
            }

            // Draw column lines.
            for (int i = 1; i < numGridLines; i++)
            {
                Line columnLine = new Line();
                columnLine.X1 = chessboardColumnWidth * i;
                columnLine.Y1 = 0;
                columnLine.X2 = chessboardColumnWidth * i;
                columnLine.Y2 = chessboardCanvas.ActualHeight;
                columnLine.StrokeThickness = 1;
                columnLine.Stroke = Brushes.Black;
                chessboardCanvas.Children.Add(columnLine);
            }
        }

        // It seems that this method cannot be refactored by data binding.
        private void InitializeChessPieceButtons(int numGridLines, double chessboardColumnWidth, double chessboardRowHeight)
        {
            for (int rowIndex = 0; rowIndex <= numGridLines; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex <= numGridLines; columnIndex++)
                {
                    // Put the button on the right position.
                    Button chessPieceButton = new Button();
                    Canvas.SetLeft(chessPieceButton, chessboardColumnWidth * columnIndex - chessboardColumnWidth / 2);
                    Canvas.SetTop(chessPieceButton, chessboardRowHeight * rowIndex - chessboardRowHeight / 2);
                    chessPieceButton.Height = chessboardRowHeight;
                    chessPieceButton.Width = chessboardColumnWidth;
                    chessPieceButton.Click += cmdPutChessPiece_Click;

                    // Set the pre-defined style for buttons of chess pieces.
                    object chessPieceStyle = Application.Current.Resources[ChessPieceStyle];
                    chessPieceButton.SetValue(Button.StyleProperty, chessPieceStyle);

                    // Add it to the canvas so that it can be active.
                    chessboardCanvas.Children.Add(chessPieceButton);

                    // Mark the button's metadata.
                    chessPieceButton.Tag = rowIndex + "," + columnIndex;
                    chessPieceButtons[rowIndex, columnIndex] = chessPieceButton;
                }
            }
        }

        // Put the chess piece to the specified position.
        private void cmdPutChessPiece_Click(object sender, RoutedEventArgs e)
        {
            if (matchStarted && canPutChessPiece)
            {
                // Get the clicked button.
                Button clickedButton = e.OriginalSource as Button;

                // Get the position of the clicked button.
                string[] positionTag = clickedButton.Tag.ToString().Split(',');
                int rowIndex = int.Parse(positionTag[0]);
                int columnIndex = int.Parse(positionTag[1]);

                // Do nothing if the black-chess-piece player put the chess piece to a forbidden position.
                if ((color == ChessPiece.Black) && CheckBalanceBreaker(rowIndex, columnIndex))
                    return;

                // Set the flag, so that this user have moved and should wait for the opponent to move.
                canPutChessPiece = false;

                // Get the corresponding style.
                object chessPieceStyle = color == ChessPiece.White
                    ? Application.Current.Resources[WhiteChessPieceStyle]
                    : Application.Current.Resources[BlackChessPieceStyle];

                // Create the chess piece circle and set its properties.
                Ellipse chessPieceCircle = new Ellipse();
                chessPieceCircles.AddLast(chessPieceCircle);
                Canvas.SetLeft(chessPieceCircle, Canvas.GetLeft(clickedButton));
                Canvas.SetTop(chessPieceCircle, Canvas.GetTop(clickedButton));
                chessPieceCircle.Width = clickedButton.ActualWidth;
                chessPieceCircle.Height = clickedButton.ActualHeight;
                chessPieceCircle.SetValue(Ellipse.StyleProperty, chessPieceStyle);

                // Remove the button so that it cannot be clicked again.
                chessboardCanvas.Children.Remove(clickedButton);

                // Add the ellipse to the canvas so that it can be seen.
                chessboardCanvas.Children.Add(chessPieceCircle);

                // Set it to null so that GC can free the space when it is free.
                chessPieceButtons[rowIndex, columnIndex] = null;

                // Send chess piece position information to the opponent.
                object chessPiecePositionInfo = new
                {
                    Sender = localAccount,
                    Receiver = opponentAccount,
                    ColumnIndex = columnIndex,
                    RowIndex = rowIndex,
                };
                Communication.Send(JsonPackageKeys.ChessPiecePosition, chessPiecePositionInfo);

                // Mark the color at the clicked position.
                chessboard[rowIndex, columnIndex] = color;

                // Judge if the user wins.
                if (Win(rowIndex, columnIndex))
                {
                    object winMessage = new
                    {
                        Sender = localAccount,
                        Receiver = opponentAccount
                    };
                    Communication.Send(JsonPackageKeys.Win, winMessage);
                    MatchOver("你赢了");
                }
            }
        }

        public void ReceiveChessPiecePositionInfo(JToken chessPiecePositionInfo)
        {
            canPutChessPiece = true;

            // Determine the opponent's chess piece style and color.
            object opponentChessPieceStyle;
            ChessPiece opponentChessPiece;
            if (color == ChessPiece.White)
            {
                opponentChessPieceStyle = Application.Current.Resources[BlackChessPieceStyle];
                opponentChessPiece = ChessPiece.Black;
            }
            else
            {
                opponentChessPieceStyle = Application.Current.Resources[WhiteChessPieceStyle];
                opponentChessPiece = ChessPiece.White;
            }

            // Get the position of the clicked button.
            int columnIndex = int.Parse(chessPiecePositionInfo[JsonPackageKeys.ColumnIndex].ToString());
            int rowIndex = int.Parse(chessPiecePositionInfo[JsonPackageKeys.RowIndex].ToString());

            // Mark the color at the clicked position.
            chessboard[rowIndex, columnIndex] = opponentChessPiece;

            // Get the clicked button.
            Button clickedButton = chessPieceButtons[rowIndex, columnIndex];

            this.Dispatcher.Invoke(() =>
            {
                // Remove the button so that it cannot be clicked again.
                chessboardCanvas.Children.Remove(clickedButton);

                // Create the chess piece circle and set its properties.
                Ellipse chessPieceCircle = new Ellipse();
                chessPieceCircles.AddLast(chessPieceCircle);
                Canvas.SetLeft(chessPieceCircle, Canvas.GetLeft(clickedButton));
                Canvas.SetTop(chessPieceCircle, Canvas.GetTop(clickedButton));
                chessPieceCircle.Width = clickedButton.ActualWidth;
                chessPieceCircle.Height = clickedButton.ActualHeight;
                chessPieceCircle.SetValue(Ellipse.StyleProperty, opponentChessPieceStyle);

                // Add the ellipse to the canvas so that it can be seen.
                chessboardCanvas.Children.Add(chessPieceCircle);
            });

            // Set it to null so that GC can free the space when it is free.
            chessPieceButtons[rowIndex, columnIndex] = null;
        }

        private bool AcceptOrder(JToken orderInfo)
        {
            string whiteChessPieceUser = orderInfo[JsonPackageKeys.WhiteChessPieceUser].ToString();
            string blackChessPieceUser = orderInfo[JsonPackageKeys.BlackChessPieceUser].ToString();

            string order = "黑棋: " + blackChessPieceUser + "\n" + "白棋: " + whiteChessPieceUser;
            MessageBoxResult result = MessageBox.Show(order, "确认顺序", MessageBoxButton.YesNo);

            return result == MessageBoxResult.Yes;
        }

        public void ResponseOrderNegotiation(JToken orderInfo)
        {
            if (AcceptOrder(orderInfo))
            {
                // Reverse the sender and receiver in the "orderInfo" object.
                object acceptOrderInfo = new
                {
                    Sender = orderInfo[JsonPackageKeys.Receiver].ToString(),
                    Receiver = orderInfo[JsonPackageKeys.Sender].ToString(),
                    WhiteChessPieceUser = orderInfo[JsonPackageKeys.WhiteChessPieceUser].ToString(),
                    BlackChessPieceUser = orderInfo[JsonPackageKeys.BlackChessPieceUser].ToString()
                };
                Communication.Send(JsonPackageKeys.AcceptOrder, acceptOrderInfo);
                StartMatch(orderInfo);
            }
            else
                this.Dispatcher.Invoke(() => new OrderNegotiationWindow(localAccount, MatchInfo).Show());
        }

        public void StartMatch(JToken orderInfo)
        {
            // Initialize the user account text.
            string sender = orderInfo[JsonPackageKeys.Sender].ToString();
            string receiver = orderInfo[JsonPackageKeys.Receiver].ToString();
            opponentAccount = sender;
            this.Dispatcher.Invoke(() =>
            {
                txtLocalAccount.Text = localAccount;
                txtOpponentAccount.Text = localAccount == sender ? receiver : sender;
            });

            // Get the account fo the black chess piece user.
            string blackChessPieceUser = orderInfo[JsonPackageKeys.BlackChessPieceUser].ToString();

            // Set user colors.
            if (blackChessPieceUser == localAccount)
            {
                this.Dispatcher.Invoke(() =>
                {
                    localAccountChessPiece.Fill = Brushes.Black;
                    opponentAccountChessPiece.Fill = Brushes.White;
                });
                color = ChessPiece.Black;
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    localAccountChessPiece.Fill = Brushes.White;
                    opponentAccountChessPiece.Fill = Brushes.Black;
                });
                color = ChessPiece.White;
            }

            // Mark the match state as started.
            matchStarted = true;

            // Black first.
            if (localAccount == blackChessPieceUser)
                canPutChessPiece = true;
        }

        // Returns true if the local account wins, otherwise, false.
        private bool Win(int rowIndex, int columnIndex)
        {
            return CheckRow(rowIndex, columnIndex) || CheckColumn(rowIndex, columnIndex) || CheckMainDiagonal(rowIndex, columnIndex) || CheckAntiDiagonal(rowIndex, columnIndex);
        }

        private int RowLineLength(int rowIndex, int columnIndex)
        {
            // Starts from 1 since the given chess piece is also a chess piece having the same color.
            int connectedChessPieces = 1;
            int adjacentColumnIndex;

            // Go left, at most 4 steps.
            // Make the column index vary consistently as (columnIndex + i).
            // Don't forget to validate the boundary of the index.
            for (int i = -1; i >= -(LineSize - 1); i--)
            {
                adjacentColumnIndex = columnIndex + i;
                if ((adjacentColumnIndex >= 0) &&
                    (adjacentColumnIndex < ChessboardSize) &&
                    (chessboard[rowIndex, adjacentColumnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            // Go right, at most 4 steps.
            // Note that the connectedChessPieces won't start from 1 again, so once it equals 5, return true immediately.
            for (int i = 1; i <= (LineSize - 1); i++)
            {
                adjacentColumnIndex = columnIndex + i;
                if ((adjacentColumnIndex >= 0) &&
                    (adjacentColumnIndex < ChessboardSize) &&
                    (chessboard[rowIndex, adjacentColumnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            return connectedChessPieces;
        }

        private bool CheckRow(int rowIndex, int columnIndex)
        {
            return RowLineLength(rowIndex, columnIndex) >= LineSize;
        }

        private int ColumnLineLength(int rowIndex, int columnIndex)
        {
            int connectedChessPieces = 1;
            int adjacentRowIndex;

            for (int i = -1; i >= -(LineSize - 1); i--)
            {
                adjacentRowIndex = rowIndex + i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, columnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            for (int i = 1; i <= (LineSize - 1); i++)
            {
                adjacentRowIndex = rowIndex + i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, columnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            return connectedChessPieces;
        }

        private bool CheckColumn(int rowIndex, int columnIndex)
        {
            return ColumnLineLength(rowIndex, columnIndex) >= LineSize;
        }

        private int MainDiagonalLength(int rowIndex, int columnIndex)
        {
            int connectedChessPieces = 1;
            int adjacentRowIndex;
            int adjacentColumnIndex;

            // Go up-left.
            for (int i = -1; i >= -(LineSize - 1); i--)
            {
                adjacentRowIndex = rowIndex + i;
                adjacentColumnIndex = columnIndex + i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (adjacentColumnIndex >= 0) &&
                    (adjacentColumnIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, adjacentColumnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            // Go down-right.
            for (int i = 1; i <= (LineSize - 1); i++)
            {
                adjacentRowIndex = rowIndex + i;
                adjacentColumnIndex = columnIndex + i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (adjacentColumnIndex >= 0) &&
                    (adjacentColumnIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, adjacentColumnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            return connectedChessPieces;
        }

        private bool CheckMainDiagonal(int rowIndex, int columnIndex)
        {
            return MainDiagonalLength(rowIndex, columnIndex) >= LineSize;
        }

        private int AntiDiagonalLength(int rowIndex, int columnIndex)
        {
            int connectedChessPieces = 1;
            int adjacentRowIndex;
            int adjacentColumnIndex;

            // Go up-right.
            // adjacentColumnIndex: increase;
            // adjacentRowIndex: decrease.
            for (int i = 1; i <= (LineSize - 1); i++)
            {
                adjacentRowIndex = rowIndex - i;
                adjacentColumnIndex = columnIndex + i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (adjacentColumnIndex >= 0) &&
                    (adjacentColumnIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, adjacentColumnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            // Go down-left.
            // adjacentColumnIndex: decrease;
            // adjacentRowIndex: increase.
            for (int i = 1; i <= (LineSize - 1); i++)
            {
                adjacentRowIndex = rowIndex + i;
                adjacentColumnIndex = columnIndex - i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (adjacentColumnIndex >= 0) &&
                    (adjacentColumnIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, adjacentColumnIndex] == color))
                    connectedChessPieces++;
                else
                    break;
            }

            return connectedChessPieces;
        }

        private bool CheckAntiDiagonal(int rowIndex, int columnIndex)
        {
            return AntiDiagonalLength(rowIndex, columnIndex) >= LineSize;
        }

        /// <summary>
        /// Checks if the position the black-chess player put the chess piece will break the game balance.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private bool CheckBalanceBreaker(int rowIndex, int columnIndex)
        {
            return (RowLineLength(rowIndex, columnIndex) > LineSize) ||
                   (ColumnLineLength(rowIndex, columnIndex) > LineSize) ||
                   (MainDiagonalLength(rowIndex, columnIndex) > LineSize) ||
                   (AntiDiagonalLength(rowIndex, columnIndex) > LineSize);
        }

        // A test method.
        private string GetChessboardState()
        {
            StringBuilder chessboardState = new StringBuilder();
            for (int i = 0; i < ChessboardSize; i++)
            {
                for (int j = 0; j < ChessboardSize; j++)
                {
                    chessboardState.Append(((int)chessboard[i, j]).ToString());
                    if (j != ChessboardSize - 1)
                        chessboardState.Append("  ");
                }

                chessboardState.Append("\n");
            }

            return chessboardState.ToString();
        }

        public void MatchOver(string matchResult)
        {
            MessageBox.Show(matchResult);
            this.Dispatcher.Invoke(() =>
            {
                //new SearchForMatchWindow(localAccount).Show();
                this.Hide();
                SearchForMatchWindow.Show();
            });
        }

        private void cmdSendMessage_Click(object sender, RoutedEventArgs e)
        {
            // Send text message to the opponent.
            Object textMessage = new
            {
                Sender = localAccount,
                Receiver = opponentAccount,
                Content = txtMessageToSend.Text
            };
            Communication.Send(JsonPackageKeys.TextMessage, textMessage);

            // Add text message to the chatting box.
            string formattedMessage = localAccount + " " + DateTime.Now + ":" + Environment.NewLine + txtMessageToSend.Text;
            lstChatting.Items.Add(formattedMessage);
            txtMessageToSend.Clear();
        }

        public void ReceiveTextMessage(JToken textMessage)
        {
            string sender = textMessage[JsonPackageKeys.Sender].ToString();
            string content = textMessage[JsonPackageKeys.Content].ToString();

            string formattedMessage = sender + " " + " " + DateTime.Now + ":" + Environment.NewLine + content;
            this.Dispatcher.Invoke(() => lstChatting.Items.Add(formattedMessage));
        }
    }
}
