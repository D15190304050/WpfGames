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
        private string localAccount;
        internal JToken matchInfo;
        private bool matchStarted;
        private bool canPutChess;
        private string opponentAccount;
        private ChessPiece color;
        internal SearchForMatchWindow searchForMatchWindow;

        // Listen incoming messages.
        private BackgroundWorker messageListener;

        public MainScene(string localAccount, JToken matchInfo)
        {
            matchStarted = false;
            canPutChess = false;
            
            chessPieceButtons = new Button[ChessboardSize, ChessboardSize];
            this.localAccount = localAccount;
            this.matchInfo = matchInfo;

            chessboard = new ChessPiece[ChessboardSize, ChessboardSize];
            for (int i = 0; i < ChessboardSize; i++)
            {
                for (int j = 0; j < ChessboardSize; j++)
                    chessboard[i, j] = ChessPiece.Blank;
            }

            messageListener = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            messageListener.DoWork += ListenOpponentMessage;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int numGridLines = ChessboardSize - 1;
            double chessboardColumnWidth = chessboardCanvas.ActualWidth / numGridLines;
            double chessboardRowHeight = chessboardCanvas.ActualHeight / numGridLines;

            DrawChessboardGridLines(numGridLines, chessboardColumnWidth, chessboardRowHeight);
            InitializeChessPieceButtons(numGridLines, chessboardColumnWidth, chessboardRowHeight);

            //messageListener.RunWorkerAsync();
        }

        public new void Show()
        {
            base.Show();
            // Negotiate the order once both players entered the game.
            if (localAccount == matchInfo[JsonPackageKeys.InitiatorAccount].ToString())
                new OrderNegotiationWindow(localAccount, matchInfo).Show();

            MessageBox.Show("Here");
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
            // i: columnIndex
            // j: rowIndex
            for (int i = 0; i <= numGridLines; i++)
            {
                for (int j = 0; j <= numGridLines; j++)
                {
                    Button chessPiece = new Button();
                    Canvas.SetLeft(chessPiece, chessboardColumnWidth * i - chessboardColumnWidth / 2);
                    Canvas.SetTop(chessPiece, chessboardRowHeight * j - chessboardRowHeight / 2);
                    chessPiece.Height = chessboardRowHeight;
                    chessPiece.Width = chessboardColumnWidth;
                    chessPiece.Click += cmdPutChessPiece_Click;

                    // Set the pre-defined style for buttons of chess pieces.
                    object chessPieceStyle = Application.Current.Resources[ChessPieceStyle];
                    chessPiece.SetValue(Button.StyleProperty, chessPieceStyle);
                    chessboardCanvas.Children.Add(chessPiece);

                    chessPiece.Tag = i + "," + j;
                    chessPieceButtons[j, i] = chessPiece;
                }
            }
        }

        // Put the chess piece to the specified position.
        private void cmdPutChessPiece_Click(object sender, RoutedEventArgs e)
        {
            if (matchStarted && canPutChess)
            {
                canPutChess = false;

                Button clickedButton = e.OriginalSource as Button;
                object chessPieceStyle;
                if (color == ChessPiece.White)
                    chessPieceStyle = Application.Current.Resources[WhiteChessPieceStyle];
                else
                    chessPieceStyle = Application.Current.Resources[BlackChessPieceStyle];

                Ellipse chessPieceCircle = new Ellipse();
                Canvas.SetLeft(chessPieceCircle, Canvas.GetLeft(clickedButton));
                Canvas.SetTop(chessPieceCircle, Canvas.GetTop(clickedButton));
                chessPieceCircle.Width = clickedButton.ActualWidth;
                chessPieceCircle.Height = clickedButton.ActualHeight;
                chessPieceCircle.SetValue(Ellipse.StyleProperty, chessPieceStyle);
                chessboardCanvas.Children.Remove(clickedButton);
                chessboardCanvas.Children.Add(chessPieceCircle);
                string[] positionTag = clickedButton.Tag.ToString().Split(',');
                int columnIndex = int.Parse(positionTag[0]);
                int rowIndex = int.Parse(positionTag[1]);

                object chessPiecePositionInfo = new
                {
                    Sender = localAccount,
                    Receiver = opponentAccount,
                    ColumnIndex = columnIndex,
                    RowIndex = rowIndex,
                };
                Communication.Send(JsonPackageKeys.ChessPiecePosition, chessPiecePositionInfo);
                chessboard[rowIndex, columnIndex] = color;

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

        private void ListenOpponentMessage(object sender, DoWorkEventArgs e)
        {
            for (;;)
            {
                JObject responseMessage = Communication.ReceiveMessage();
                switch (responseMessage[JsonPackageKeys.Type].ToString())
                {
                    case JsonPackageKeys.Order:
                        ResponseOrderNegotiation(responseMessage[JsonPackageKeys.Body]);
                        break;
                    case JsonPackageKeys.AcceptOrder:
                        StartMatch(responseMessage[JsonPackageKeys.Body]);
                        break;
                    case JsonPackageKeys.ChessPiecePosition:
                        ReceiveChessPiecePositionInfo(responseMessage[JsonPackageKeys.Body]);
                        break;
                    case JsonPackageKeys.Win:
                        MatchOver("你输了");
                        break;
                }
            }
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
                new OrderNegotiationWindow(localAccount, matchInfo).ShowDialog();
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

            string whiteChessPieceUser = orderInfo[JsonPackageKeys.WhiteChessPieceUser].ToString();
            string blackChessPieceUser = orderInfo[JsonPackageKeys.BlackChessPieceUser].ToString();

            if (whiteChessPieceUser == localAccount)
            {
                this.Dispatcher.Invoke(() =>
                {
                    localAccountChessPiece.Fill = Brushes.White;
                    opponentAccountChessPiece.Fill = Brushes.Black;
                });
                color = ChessPiece.White;
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    localAccountChessPiece.Fill = Brushes.Black;
                    opponentAccountChessPiece.Fill = Brushes.White;
                });
                color = ChessPiece.Black;
            }

            matchStarted = true;
            if (localAccount == blackChessPieceUser)
                canPutChess = true;
        }

        public void ReceiveChessPiecePositionInfo(JToken chessPiecePositionInfo)
        {
            object chessPieceStyle = color == ChessPiece.White ? Application.Current.Resources[BlackChessPieceStyle] : Application.Current.Resources[WhiteChessPieceStyle];

            int columnIndex = int.Parse(chessPiecePositionInfo[JsonPackageKeys.ColumnIndex].ToString());
            int rowIndex = int.Parse(chessPiecePositionInfo[JsonPackageKeys.RowIndex].ToString());
            this.Dispatcher.Invoke(() =>
            {
                ChessPiece opponentChessPiece = color == ChessPiece.White ? ChessPiece.Black : ChessPiece.White;
                chessboard[rowIndex, columnIndex] = opponentChessPiece;
                chessboardCanvas.Children.Remove(chessPieceButtons[rowIndex, columnIndex]);
                Button clickedButton = chessPieceButtons[rowIndex, columnIndex];
                Ellipse chessPieceCircle = new Ellipse();
                Canvas.SetLeft(chessPieceCircle, Canvas.GetLeft(clickedButton));
                Canvas.SetTop(chessPieceCircle, Canvas.GetTop(clickedButton));
                chessPieceCircle.Width = clickedButton.ActualWidth;
                chessPieceCircle.Height = clickedButton.ActualHeight;
                chessPieceCircle.SetValue(Ellipse.StyleProperty, chessPieceStyle);
                chessboardCanvas.Children.Remove(clickedButton);
                chessboardCanvas.Children.Add(chessPieceCircle);
            });
            chessPieceButtons[rowIndex, columnIndex] = null;
            canPutChess = true;
        }

        // Returns true if the local account wins, otherwise, false.
        private bool Win(int rowIndex, int columnIndex)
        {
            return CheckRow(rowIndex, columnIndex) || CheckColumn(rowIndex, columnIndex) || CheckMainDiagonal(rowIndex, columnIndex) || CheckAntiDiagonal(rowIndex, columnIndex);
        }

        private bool CheckRow(int rowIndex, int columnIndex)
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
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
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
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
                else
                    break;
            }

            return false;
        }

        private bool CheckColumn(int rowIndex, int columnIndex)
        {
            int connectedChessPieces = 1;
            int adjacentRowIndex;

            for (int i = -1; i >= -(LineSize - 1); i--)
            {
                adjacentRowIndex = rowIndex + i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, columnIndex] == color))
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
                else
                    break;
            }

            for (int i = 1; i <= (LineSize - 1); i++)
            {
                adjacentRowIndex = rowIndex + i;
                if ((adjacentRowIndex >= 0) &&
                    (adjacentRowIndex < ChessboardSize) &&
                    (chessboard[adjacentRowIndex, columnIndex] == color))
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
                else
                    break;
            }

            return false;
        }

        private bool CheckMainDiagonal(int rowIndex, int columnIndex)
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
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
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
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
                else
                    break;
            }

            return false;
        }

        private bool CheckAntiDiagonal(int rowIndex, int columnIndex)
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
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
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
                {
                    connectedChessPieces++;
                    if (connectedChessPieces == LineSize)
                        return true;
                }
                else
                    break;
            }

            return false;
        }

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

        public void MatchOver(string matchState)
        {
            MessageBox.Show(matchState);
            this.Dispatcher.Invoke(() =>
            {
                new SearchForMatchWindow(localAccount).Show();
                this.Hide();
                searchForMatchWindow.Show();
            });
        }
    }
}
