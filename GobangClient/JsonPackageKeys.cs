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
        public const string Register = "Register";
        public const string Account = "Account";
        public const string Password = "Password";
        public const string MailAddress = "MailAddress";
        public const string AccountAlreadyExists = "用户名已存在";
        public const string Login = "Login";
        public const string Error = "Error";
        public const string Success = "Success";
        public const string DetailedError = "DetailedError";
        public const string NoSuchAccount = "该用户不存在";
        public const string WrongPassword = "密码错误";
        public const string ValidateAccount = "ValidateAccount";
        public const string WrongMailAddress = "邮箱错误";
        public const string Empty = "Empty";
        public const string BlankNotAllowed = "用户名和密码都不能为空";
        public const string ModifyPassword = "ModifyPassword";
        public const string UnknownError = "未知错误";
        public const string PasswordConsistencyError = "2次输入的密码不一致";
        public const string RequestForUserList = "RequestForUserList";
        public const string UserList = "UserList";
        public const string IdleUserCount = "IdleUserCount";
        public const string PlayingUserCount = "PlayingUserCount";
        public const string IdleUsers = "IdleUsers";
        public const string PlayingUsers = "PlayingUsers";
        public const string RequestForMatch = "RequestForMatch";
        public const string InitiatorAccount = "InitiatorAccount";
        public const string OpponentAccount = "OpponentAccount";
        public const string OpponentNotAvailable = "该用户正在游戏中";
        public const string AcceptMatch = "AcceptMatch";
        public const string RejectMatch = "RejectMatch";
        public const string BlackChessPieceUser = "BlackChessPieceUser";
        public const string WhiteChessPieceUser = "WhiteChessPieceUser";
        public const string Order = "Order";
        public const string AcceptOrder = "AcceptOrder";
        public const string ReNegotiateOrder = "ReNegotiateOrder";
        public const string Sender = "Sender";
        public const string Receiver = "Receiver";
        public const string ChessPiecePosition = "ChessPiecePosition";
        public const string ColumnIndex = "ColumnIndex";
        public const string RowIndex = "RowIndex";
    }
}
