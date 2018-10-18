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
        private Chessboard chessboard;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayerMove(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button) e.OriginalSource;
            string content = cmd.Tag.ToString();
            string[] position = content.Split(',');
            int rowIndex = int.Parse(position[0]);
            int columnIndex = int.Parse(position[1]);

            Image mark = new Image();
            mark.Source = new BitmapImage(new Uri("Images/o.png", UriKind.Relative));
            mark.MaxHeight = 150;
            mark.MaxWidth = 150;
            cmd.Content = mark;
        }

        private void cmdStartGame_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
