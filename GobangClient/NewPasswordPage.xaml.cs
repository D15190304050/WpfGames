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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GobangClient
{
    /// <summary>
    /// Interaction logic for NewPasswordPage.xaml
    /// </summary>
    public partial class NewPasswordPage : Page
    {
        public NewPasswordPage()
        {
            InitializeComponent();
        }

        public JObject ApplyNewPassword(AccountInfo account)
        {
            if (gridModifyPassword.DataContext is PasswordInfo password)
            {
                if ((pbNewPassword.Password.Length == 0) ||
                    (pbConfirmPassword.Password.Length == 0) ||
                    (password.NewPassword != pbNewPassword.Password) ||
                    (password.ConfirmPassword != pbConfirmPassword.Password))
                    return JObject.FromObject(new
                    {
                        Type = JsonPackageKeys.Empty,
                        Body = ""
                    });

                if (pbNewPassword.Password != pbConfirmPassword.Password)
                {
                    return JObject.FromObject(new
                    {
                        Type = JsonPackageKeys.Error,
                        Body = new
                        {
                            DetailedError = JsonPackageKeys.PasswordConsistencyError
                        }
                    });
                }

                account.Password = Encrypter.Encrypt(password.NewPassword);
                Communication.Send(JsonPackageKeys.ModifyPassword, account);

                return Communication.Receive()[0];
            }

            return JObject.FromObject(new
            {
                Type = JsonPackageKeys.Error,
                Body = new
                {
                    DetailedError = JsonPackageKeys.UnknownError
                }
            });
        }
    }
}
