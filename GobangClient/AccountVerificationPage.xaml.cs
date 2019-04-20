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

        /// <summary>
        /// 验证用户与邮箱是否匹配。
        /// </summary>
        /// <returns></returns>
        public JObject ValidateMailAddress()
        {
            // 禁止为空。
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

            // 将用户填写的信息上传给服务器，让服务器进行验证。
            object messageBody = new
            {
                Account = txtAccount.Text,
                MailAddress = txtMailAddress.Text
            };
            Communication.Send(JsonPackageKeys.ValidateAccount, messageBody);

            // 获取并返回服务器的验证结果。
            return Communication.ReceiveMessage();
        }
    }
}
