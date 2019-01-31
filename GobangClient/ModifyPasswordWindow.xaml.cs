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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for ModifyPasswordWindow.xaml
    /// </summary>
    public partial class ModifyPasswordWindow : Window
    {
        private AccountVerificationPage accountVerificationPage;
        private NewPasswordPage newPasswordPage;
        private AccountInfo account;

        public ModifyPasswordWindow()
        {
            account = new AccountInfo();
            InitializeComponent();
        }

        private void cmdLast_Click(object sender, RoutedEventArgs e)
        {
            frameSteps.Content = accountVerificationPage;
            cmdNext.IsEnabled = true;
            cmdFinish.IsEnabled = false;
            cmdLast.IsEnabled = false;
        }

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {
            JObject responseMessage = accountVerificationPage.ValidateMailAddress();
            switch (responseMessage[JsonPackageKeys.Type].ToString())
            {
                case JsonPackageKeys.Success:
                    frameSteps.Content = newPasswordPage;
                    account.Account = accountVerificationPage.txtAccount.Text;
                    cmdNext.IsEnabled = false;
                    cmdLast.IsEnabled = true;
                    cmdFinish.IsEnabled = true;
                    break;
                case JsonPackageKeys.Error:
                    DisplayErrorMessage(responseMessage);
                    break;
                default:
                    MessageBox.Show("未知错误\n" + responseMessage);
                    break;
            }
        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {
            JObject responseMessage = newPasswordPage.ApplyNewPassword(account);
            switch (responseMessage[JsonPackageKeys.Type].ToString())
            {
                case JsonPackageKeys.Success:
                    MessageBox.Show("修改成功");
                    this.Close();
                    break;

                // Do nothing if there is some input error.
                case JsonPackageKeys.Empty:
                    break;

                case JsonPackageKeys.Error:
                    DisplayErrorMessage(responseMessage);
                    break;

                default:
                    MessageBox.Show(JsonPackageKeys.UnknownError + "\n" + responseMessage);
                    break;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            accountVerificationPage = new AccountVerificationPage();
            newPasswordPage = new NewPasswordPage();
            frameSteps.Content = accountVerificationPage;
        }

        // Use a method to encapsulate this function to enhance the readability.
        private void DisplayErrorMessage(JObject responseMessage)
        {
            txtErrorMessage.Text = responseMessage[JsonPackageKeys.Body][JsonPackageKeys.DetailedError].ToString();
        }
    }
}
