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
using System.Threading;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
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
                txtErrorMessage.Text = JsonPackageKeys.BlankNotAllowed;
            else
            {
                txtErrorMessage.Text = "";

                accountToCommit.Account = txtAccount.Text;
                accountToCommit.Password = Encrypter.Encrypt(passwordBox.Password);
                accountToCommit.MailAddress = "";
                Communication.Send(JsonPackageKeys.Login, accountToCommit);
                
                JObject[] responseMessages = Communication.Receive();
                for (int i = 0; i < responseMessages.Length; i++)
                {
                    JObject responseMessage = responseMessages[i];
                    switch (responseMessage[JsonPackageKeys.Type].ToString())
                    {
                        case JsonPackageKeys.Error:
                            txtErrorMessage.Text = responseMessage[JsonPackageKeys.Body][JsonPackageKeys.DetailedError].ToString();
                            break;
                        case JsonPackageKeys.Success:
                            // Use Show() method so that the windows will not be blocked in the join test.
                            // In the standalone release part, the ShowDialog() method should be used.
                            new SearchForGameWindow(accountToCommit.Account).Show();
                            this.Close();
                            break;
                        default:
                            MessageBox.Show(JsonPackageKeys.UnknownError + "\n" + responseMessage);
                            break;
                    }
                }
            }
        }

        private void cmdForgetPassword_Click(object sender, RoutedEventArgs e)
        {
            new ModifyPasswordWindow().ShowDialog();
        }

        // This method is necessary only when this window is tested in other projects.
        // The following code will deal with the path mapping problem.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Background = new ImageBrush(new BitmapImage(new Uri("Images/Login_Background.jpg", UriKind.Relative)));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Use Show() method so that the windows will not be blocked in the join test.
            // In the standalone release part, the ShowDialog() method should be used.
            //new SearchForGameWindow(accountToCommit.Account).Show();
        }
    }
}
