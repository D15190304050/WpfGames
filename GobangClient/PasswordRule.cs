using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GobangClient
{
    public class PasswordRule : ValidationRule
    {
        // Validates following rules:
        // 长度8-20个字符，仅允许字母和数字，不允许纯数字.
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = value as string;

            if (password == null)
                return new ValidationResult(false, "密码不能为空");

            if (password.Length < 8)
                return new ValidationResult(false, "密码必须有至少8个字符");

            if (password.Length > 20)
                return new ValidationResult(false, "密码不能超过20个字符");

            bool allDigits = true;
            foreach (char c in password)
            {
                if (!char.IsLetterOrDigit(c))
                    return new ValidationResult(false, "密码仅允许输入字符或数字");

                if (char.IsLetter(c))
                    allDigits = false;
            }

            if (allDigits)
                return new ValidationResult(false, "密码不允许是纯数字");

            return new ValidationResult(true, null);
        }
    }
}
