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

        private ChessPiece[,] chessboard;

        public MainScene()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double chessboardColumnWidth = canvasChessboard.ActualWidth / ChessboardSize;
            double chessboardRowHeight = canvasChessboard.ActualHeight / ChessboardSize;

            // Draw row lines.
            for (int i = 1; i < ChessboardSize; i++)
            {
                Line rowLine = new Line();
                rowLine.X1 = 0;
                rowLine.Y1 = chessboardRowHeight * i;
                rowLine.X2 = canvasChessboard.ActualWidth;
                rowLine.Y2 = chessboardRowHeight * i;
                rowLine.StrokeThickness = 1;
                rowLine.Stroke = Brushes.Black;
                canvasChessboard.Children.Add(rowLine);
            }

            // Draw column lines.
            for (int i = 1; i < ChessboardSize; i++)
            {
                Line columnLine = new Line();
                columnLine.X1 = chessboardColumnWidth * i;
                columnLine.Y1 = 0;
                columnLine.X2 = chessboardColumnWidth * i;
                columnLine.Y2 = canvasChessboard.ActualHeight;
                columnLine.StrokeThickness = 1;
                columnLine.Stroke = Brushes.Black;
                canvasChessboard.Children.Add(columnLine);
            }
        }
    }
}
