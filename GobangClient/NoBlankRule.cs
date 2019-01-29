using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GobangClient
{
    public class NoBlankRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string content = value as string;

            if ((content == null) || (content.Length == 0))
                return new ValidationResult(false, "此项内容不允许为空");

            return new ValidationResult(true, "");
        }
    }
}
