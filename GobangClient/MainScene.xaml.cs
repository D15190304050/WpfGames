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

        private ChessPiece[,] chessboard;


        public MainScene()
        {
            chessboard = new ChessPiece[ChessboardSize, ChessboardSize];

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int numGridLines = ChessboardSize - 1;
            double chessboardColumnWidth = chessboardCanvas.ActualWidth / numGridLines;
            double chessboardRowHeight = chessboardCanvas.ActualHeight / numGridLines;

            DrawChessboardGridLines(numGridLines, chessboardColumnWidth, chessboardRowHeight);
            DrawChessPieceButtons(numGridLines, chessboardColumnWidth, chessboardRowHeight);
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
        private void DrawChessPieceButtons(int numGridLines, double chessboardColumnWidth, double chessboardRowHeight)
        {
            for (int i = 0; i <= numGridLines; i++)
            {
                for (int j = 0; j <= numGridLines; j++)
                {
                    Button chessPiece = new Button();
                    Canvas.SetLeft(chessPiece, chessboardColumnWidth * i - chessboardColumnWidth / 2);
                    Canvas.SetTop(chessPiece, chessboardRowHeight * j - chessboardRowHeight / 2);
                    chessPiece.Height = chessboardRowHeight;
                    chessPiece.Width = chessboardColumnWidth;

                    // Set the pre-defined style for buttons of chess pieces.
                    chessPiece.SetValue(Button.StyleProperty, Application.Current.Resources[ChessPieceStyle]);
                    chessboardCanvas.Children.Add(chessPiece);

                    chessPiece.Tag = i + "," + j;
                }
            }
        }
    }
}
