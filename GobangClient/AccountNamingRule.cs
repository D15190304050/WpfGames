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
    /// <summary>
    /// 用户名的验证规则。具体规则为：长度3-20个字符，仅允许字母和数字，以字母开头，不允许与已注册用户重复。
    /// </summary>
    public class AccountNamingRule : ValidationRule
    {
        /// <summary>
        /// 验证用户名。
        /// </summary>
        /// <param name="value">要验证的值。</param>
        /// <param name="cultureInfo">地区文化信息。</param>
        /// <returns>用于表示验证结果的<see cref="ValidationResult" />对象。</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // 将用户名从object转换成string类型。
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
