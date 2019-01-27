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

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void cmdRegister_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ValidateInput()
        {
            string account = txtAccount.Text;
            string password = pbPassword.Password;
            string mailAddress = txtMailAddress.Text;

            //if (account.Length < 3 || account.)

        }

        // Calculates and returns the MD5 hash value of the specified string password.
        public static string Encrypt(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder encryptedPassword = new StringBuilder();
            foreach (byte b in md5Bytes)
                encryptedPassword.Append(b.ToString("X2"));

            return encryptedPassword.ToString();
        }
    }
}
