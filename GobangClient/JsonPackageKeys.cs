using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GobangClient
{
    public static class JsonPackageKeys
    {
        public const string Type = "Type";
        public const string Body = "Body";
        public const string Error = "Error";
        public const string DetailedError = "DetailedError";
        public const string AccountAlreadyExists = "Account already exists";
        public const string Success = "Success";
        public const string Login = "Login";
        public const string Register = "Register";
        public const string NoSuchAccount = "该用户不存在";
        public const string WrongPassword = "密码错误";
        public const string ValidateAccount = "ValidateAccount";
        public const string WrongMailAddress = "邮箱错误";
        public const string Empty = "Empty";
        public const string BlankNotAllowed = "用户名和密码都不能为空";
        public const string ModifyPassword = "ModifyPassword";
        public const string UnknownError = "未知错误";
        public const string PasswordConsistencyError = "2次输入的密码不一致";
    }
}
