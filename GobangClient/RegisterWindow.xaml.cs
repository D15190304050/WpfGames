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

        private AccountInfo commitAccountInfo;
        private static MD5 md5Encryptor;
        private byte[] sendBuffer;
        private byte[] receiveBuffer;

        static RegisterWindow()
        {
            md5Encryptor = new MD5CryptoServiceProvider();
        }

        public RegisterWindow()
        {
            commitAccountInfo = new AccountInfo();
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
                if ((registerAccountInfo.Account != txtAccount.Text) ||
                    (registerAccountInfo.Password != passwordBox.Password) ||
                    (registerAccountInfo.MailAddress != txtMailAddress.Text))
                    return;

                commitAccountInfo.Account = registerAccountInfo.Account;
                commitAccountInfo.Password = Encrypt(registerAccountInfo.Password);
                commitAccountInfo.MailAddress = registerAccountInfo.MailAddress;

                JObject accountJson = JObject.FromObject(new
                {
                    Type = "Register",
                    Body = commitAccountInfo
                });

                byte[] accountBytes = Encoding.UTF8.GetBytes(accountJson.ToString());
                App.ClientSocket.Send(accountBytes);

                int receivedLength = App.ClientSocket.Receive(receiveBuffer);
                string responseText = Encoding.UTF8.GetString(receiveBuffer, 0, receivedLength);
                JObject responseMessage = JObject.Parse(responseText);

                switch (responseMessage[JsonPackageKeys.Type].ToString())
                {
                    // Display error message if an error occured.
                    case JsonPackageKeys.Error:
                        DisplayErrorMessage(responseMessage);
                        break;

                    case JsonPackageKeys.Success:
                        MessageBox.Show("注册成功");
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
            txtErrorMessage.Text = responseMessage[JsonPackageKeys.Body][JsonPackageKeys.DetailedMessage].ToString();
        }

        // Calculates and returns the MD5 hash value of the specified string password.
        public static string Encrypt(string password)
        {
            byte[] md5Bytes = md5Encryptor.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder encryptedPassword = new StringBuilder();
            foreach (byte b in md5Bytes)
                encryptedPassword.Append(b.ToString("X2"));

            return encryptedPassword.ToString();
        }
    }
}
