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

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// By convention, Gobang is a game played on 15*15 chessboard by two players.
        /// </summary>
        private const int ChessboardSize = 15;

        private ChessPiece[,] chessboard;


        public MainWindow()
        {
            chessboard = new ChessPiece[ChessboardSize, ChessboardSize];

            InitializeComponent();
        }
    }
}
