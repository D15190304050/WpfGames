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
    /// Interaction logic for AccountVerificationPage.xaml
    /// </summary>
    public partial class AccountVerificationPage : Page
    {
        public AccountVerificationPage()
        {
            InitializeComponent();
        }

        public JObject ValidateMailAddress()
        {
            if (txtAccount.Text.Length == 0 || txtMailAddress.Text.Length == 0)
            {
                return JObject.FromObject(new
                {
                    Type = JsonPackageKeys.Error,
                    Body = new
                    {
                        DetailedError = JsonPackageKeys.BlankNotAllowed
                    }
                });
            }

            object messageBody = new
            {
                Account = txtAccount.Text,
                MailAddress = txtMailAddress.Text
            };
            Communication.Send(JsonPackageKeys.ValidateAccount, messageBody);

            return Communication.ReceiveMessage();
        }
    }
}
