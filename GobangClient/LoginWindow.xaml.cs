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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private const string BlankNotAllowed = "用户名和密码都不能为空";
        private AccountInfo accountToCommit;

        public LoginWindow()
        {
            InitializeComponent();

            accountToCommit = new AccountInfo();

            try
            {
                Communication.Start();
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
            if (txtAccount.Text.Length == 0 || passwordBox.Password.Length == 0)
                txtErrorMessage.Text = BlankNotAllowed;
            else
            {
                txtErrorMessage.Text = "";

                accountToCommit.Account = txtAccount.Text;
                accountToCommit.Password = Encrypter.Encrypt(passwordBox.Password);
                accountToCommit.MailAddress = "";
                Communication.Send(JsonPackageKeys.Login, accountToCommit);

                JObject responseMessage = Communication.Receive();
                switch (responseMessage[JsonPackageKeys.Type].ToString())
                {
                    case JsonPackageKeys.Error:
                        txtErrorMessage.Text = responseMessage[JsonPackageKeys.Body][JsonPackageKeys.DetailedError].ToString();
                        break;
                    case JsonPackageKeys.Success:
                        Window window = new MainScene();
                        window.Show();
                        this.Close();
                        break;
                    default:
                        MessageBox.Show("未知错误\n" + responseMessage);
                        break;
                }
            }
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
