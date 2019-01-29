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
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private const int BufferSize = 1024;

        private AccountInfo accountToCommit;
        private byte[] sendBuffer;
        private byte[] receiveBuffer;

        public RegisterWindow()
        {
            accountToCommit = new AccountInfo();
            sendBuffer = new byte[BufferSize];
            receiveBuffer = new byte[BufferSize];

            InitializeComponent();
        }

        private void cmdRegister_Click(object sender, RoutedEventArgs e)
        {
            if (gridAccountInfo.DataContext is AccountInfo registerAccountInfo)
            {
                // registerAccountInfo only contains the latest valid version of user input, while the input controls have the latest (new necessarily valid) user input.
                // Only update register information when they are equal, i.e. the current user input is valid.
                if ((registerAccountInfo.Account.Length == 0) ||
                    (registerAccountInfo.Password.Length == 0) ||
                    (registerAccountInfo.MailAddress.Length == 0)||
                    (registerAccountInfo.Account != txtAccount.Text) ||
                    (registerAccountInfo.Password != passwordBox.Password) ||
                    (registerAccountInfo.MailAddress != txtMailAddress.Text))
                    return;

                accountToCommit.Account = registerAccountInfo.Account;
                accountToCommit.Password = Encrypter.Encrypt(registerAccountInfo.Password);
                accountToCommit.MailAddress = registerAccountInfo.MailAddress;
                Communication.Send(JsonPackageKeys.Register, accountToCommit);

                JObject responseMessage = Communication.Receive();
                switch (responseMessage[JsonPackageKeys.Type].ToString())
                {
                    // Display error message if an error occured.
                    case JsonPackageKeys.Error:
                        DisplayErrorMessage(responseMessage);
                        break;

                    case JsonPackageKeys.Success:
                        MessageBox.Show("注册成功");
                        this.Close();
                        break;

                    default:
                        MessageBox.Show("未知错误" + responseMessage);
                        break;
                }
            }
        }

        // Use a method to encapsulate this function to enhance the readability.
        private void DisplayErrorMessage(JObject responseMessage)
        {
            txtErrorMessage.Text = responseMessage[JsonPackageKeys.Body][JsonPackageKeys.DetailedError].ToString();
        }
    }
}
