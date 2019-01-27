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


        public LoginWindow()
        {
            InitializeComponent();

            IPAddress serverIPAddress = IPAddress.Parse(ServerIPAddress);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, ServerPort);

            try
            {
                //App.ClientSocket.BeginConnect(serverEndPoint, Connect, null);
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
                App.ClientSocket.EndConnect(asyncResult);
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void cmdRegister_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().ShowDialog();
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

        // This method is necessary only when this window is tested in other projects.
        // The following code will deal with the path mapping problem.
        // 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri("Images/Login_Background.jpg", UriKind.Relative)));
        }
    }
}
