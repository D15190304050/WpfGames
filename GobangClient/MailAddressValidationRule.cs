using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GobangClient
{
    public class MailAddressValidationRule : ValidationRule
    {
        private static readonly Regex MailAddressRegex;

        static MailAddressValidationRule()
        {
            // Beginning with a combination of letters and digits: ^[a-zA-Z0-9_.-]+
            // An "@": @
            // 和最后一个点（.）之间必须有内容且只能是字母（大小写）、数字、点（.）、减号（-），且两个点不能挨着 [a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*
            // 最后一个点（.）之后必须有内容且内容只能是字母（大小写）、数字且长度为大于等于2个字节，小于等于6个字节 \.[a-zA-Z0-9]{2,6}$
            MailAddressRegex = new Regex(@"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$");
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string mailAddress = value as string;

            if (mailAddress == null)
                return new ValidationResult(false, "邮箱地址不能为空");

            if (!MailAddressRegex.IsMatch(mailAddress))
                return new ValidationResult(false, "请输入正确的邮箱地址");

            return new ValidationResult(true, null);
        }
    }
}
