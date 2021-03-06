﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GobangServer
{
    // Executes database commands.
    // Given that all parameters are validated before passing to methods in this class, parameter validation are no longer needed in this class.
    public static class SqlExecutor
    {
        private const string ConnectionString = @"Server = DESKTOP-5F10G1P\SQLEXPRESS; Integrated Security = True; Database = Gobang;";

        public static SqlConnection Connection { get; }

        static SqlExecutor()
        {
            Connection = new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Returns a boolean value indicating whether the specified user account exists.
        /// </summary>
        /// <returns></returns>
        public static bool Exists(string account)
        {
            bool exist;
            string cmdSelectText = "SELECT Account FROM Users WHERE Account = '" + account + "';";
            SqlCommand cmdSelect = new SqlCommand(cmdSelectText, Connection);

            // No try-catch-finally here, exception will be thrown to the caller of this method.
            Connection.Open();
            SqlDataReader reader = cmdSelect.ExecuteReader();
            exist = reader.HasRows;
            reader.Close();
            Connection.Close();

            return exist;
        }

        public static void CreateAccount(string account, string password, string mailAddress)
        {
            string cmdInsertText = "INSERT INTO Users VALUES ('" + account + "', '" + password + "', '" + mailAddress + "');";
            SqlCommand cmdInsert = new SqlCommand(cmdInsertText, Connection);

            Connection.Open();
            cmdInsert.ExecuteNonQuery();
            Connection.Close();
        }

        public static bool ValidatePassword(string account, string password)
        {
            string cmdSelectText = "SELECT Password FROM Users WHERE Account = '" + account + "';";
            SqlCommand cmdSelect = new SqlCommand(cmdSelectText, Connection);

            // No try-catch-finally here, exception will be thrown to the caller of this method.
            Connection.Open();
            SqlDataReader reader = cmdSelect.ExecuteReader();
            reader.Read();
            string realPassword = reader[0].ToString();
            reader.Close();
            Connection.Close();

            return password == realPassword;
        }

        public static bool ValidateMailAddress(string account, string mailAddress)
        {
            string cmdSelectText = "SELECT MailAddress FROM Users WHERE Account = '" + account + "';";
            SqlCommand cmdSelect = new SqlCommand(cmdSelectText, Connection);

            // No try-catch-finally here, exception will be thrown to the caller of this method.
            Connection.Open();
            SqlDataReader reader = cmdSelect.ExecuteReader();
            reader.Read();
            string readMailAddress = reader[0].ToString();
            reader.Close();
            Connection.Close();

            return mailAddress == readMailAddress;
        }

        public static void ModifyPassword(string account, string newPassword)
        {
            string cmdUpdateText = "UPDATE Users SET Password = '" + newPassword + "' WHERE Account = '" + account + "';";
            SqlCommand cmdUpdate = new SqlCommand(cmdUpdateText, Connection);

            // No try-catch-finally here, exception will be thrown to the caller of this method.
            Connection.Open();
            cmdUpdate.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
