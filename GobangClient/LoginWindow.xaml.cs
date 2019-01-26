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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private const string ServerIPAddress = "223.2.16.234";
        private const int ServerPort = 8086;

        private Socket clientSocket;

        public LoginWindow()
        {
            InitializeComponent();

            IPAddress serverIPAddress = IPAddress.Parse(ServerIPAddress);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerPort);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.BeginConnect(serverEndPoint, Connect, null);
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Connect(IAsyncResult asyncResult)
        {
            try
            {
                clientSocket.EndConnect(asyncResult);
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void cmdRegister_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdLogin_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainScene();
            window.Show();
            this.Close();
        }

        private void cmdForgetPassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
