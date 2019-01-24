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
using System.Net;
using System.Net.Sockets;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ServerIPAddress = "";
        private const int ServerPort = 8086;

        private Socket clientSocket;

        public MainWindow()
        {
            InitializeComponent();

            //IPAddress serverIPAddress = IPAddress.Parse(ServerIPAddress);
            //IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerPort);

            //try
            //{
            //    clientSocket.Connect(serverEndPoint);
            //}
            //catch (SocketException e)
            //{
            //    MessageBox.Show(e.Message);
            //}
        }

        private void cmdRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdLogin_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainScene();
            window.Show();
        }

        private void cmdForgetPassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
