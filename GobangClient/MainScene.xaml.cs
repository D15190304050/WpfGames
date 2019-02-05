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

        private const string ChessPieceStyle = "ChessPieceStyle";
        private const string BlackChessPieceStyle = "BlackChessPieceStyle";
        private const string WhiteChessPieceStyle = "WhiteChessPieceStyle";
        private const bool White = true;
        private const bool Black = false;

        private ChessPiece[,] chessboard;
        private Button[,] chessPieceButtons;
        private string localAccount;
        private JToken matchInfo;
        private bool matchStarted;
        private bool canPutChess;
        private bool color;
        private string opponentAccount;

        // Listen incoming messages.
        private BackgroundWorker messageListener;

        public MainScene(string localAccount, JToken matchInfo)
        {
            matchStarted = false;
            canPutChess = false;
            chessboard = new ChessPiece[ChessboardSize, ChessboardSize];
            chessPieceButtons = new Button[ChessboardSize, ChessboardSize];
            this.localAccount = localAccount;
            this.matchInfo = matchInfo;
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

            messageListener.RunWorkerAsync();

            // Negotiate the order once both players entered the game.
            if (localAccount == matchInfo[JsonPackageKeys.InitiatorAccount].ToString())
                new OrderNegotiationWindow(localAccount, matchInfo).Show();
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
                ChessPiece chessPiece;
                if (color == White)
                {
                    chessPieceStyle = Application.Current.Resources[WhiteChessPieceStyle];
                    chessPiece = ChessPiece.White;
                }
                else
                {
                    chessPieceStyle = Application.Current.Resources[BlackChessPieceStyle];
                    chessPiece = ChessPiece.White;
                }

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
                chessboard[columnIndex, rowIndex] = chessPiece;

                MessageBox.Show(columnIndex + ", " + rowIndex);
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

        private void ResponseOrderNegotiation(JToken orderInfo)
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

        private void StartMatch(JToken orderInfo)
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
                color = White;
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    localAccountChessPiece.Fill = Brushes.Black;
                    opponentAccountChessPiece.Fill = Brushes.White;
                });
                color = Black;
            }

            matchStarted = true;
            if (localAccount == blackChessPieceUser)
                canPutChess = true;
        }

        private void ReceiveChessPiecePositionInfo(JToken chessPiecePositionInfo)
        {
            object chessPieceStyle = color == White ? Application.Current.Resources[BlackChessPieceStyle] : Application.Current.Resources[WhiteChessPieceStyle];

            int columnIndex = int.Parse(chessPiecePositionInfo[JsonPackageKeys.ColumnIndex].ToString());
            int rowIndex = int.Parse(chessPiecePositionInfo[JsonPackageKeys.RowIndex].ToString());
            this.Dispatcher.Invoke(() =>
            {
                ChessPiece chessPiece = color == White ? ChessPiece.Black : ChessPiece.White;
                chessboard[rowIndex, columnIndex] = chessPiece;
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
    }
}
