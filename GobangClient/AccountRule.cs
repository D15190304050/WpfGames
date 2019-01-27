using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace GobangClient
{
    public class AccountRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            MessageBox.Show("123");

            string account = value as string;

            if (account == null)
                return new ValidationResult(false, "用户名不能为空");
            if (account.Length < 3)
                return new ValidationResult(false, "用户名必须有至少3个字符");
            if (account.Length > 20)
                return new ValidationResult(false, "用户名不能超过20个字符");

            if (!char.IsLetter(account[0]))
                return new ValidationResult(false, "用户名必须以字符开头");

            for (int i = 1; i < account.Length; i++)
            {
                if (!char.IsLetterOrDigit(account[i]))
                    return new ValidationResult(false, "用户名仅允许输入字符或数字");
            }

            return new ValidationResult(true, null);
        }
    }
}
