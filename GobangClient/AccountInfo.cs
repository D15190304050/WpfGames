using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GobangClient
{
    /// <summary>
    /// 表示一个用户注册时填写的相关信息。
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// 该用户的账号。
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 该用户的密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 该用户的邮箱地址。
        /// </summary>
        public string MailAddress { get; set; }
    }
}
