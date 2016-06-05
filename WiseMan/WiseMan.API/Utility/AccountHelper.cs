using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiseMan.API.Data;
using WiseMan.API.Models;

namespace WiseMan.API.Utility
{
    public static class AccountHelper
    {

        internal static void RegisterNewAccount(string username, string password, string email)
        {    
            int size = new Random().Next(10, 20);
            string salt = CreateSalt(size);
            string hashedPass = GenerateSHA256Hash(password, salt);
            try
            {
                using(Data.WiseManEntities db = new Data.WiseManEntities())
                {
                    db.RegisterNewAccount(username, hashedPass, salt, email);
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       
        internal static bool ValidateUser(string username, string password, out User user)
        {
            bool isValid = false;
            
            GetUserSaltHash_Result result;
            using(Data.WiseManEntities db = new WiseManEntities())
            {
                result = db.GetUserSaltHash(username).FirstOrDefault();
            }

            string checkHashedPass = GenerateSHA256Hash(password, result.Salt);
            if (checkHashedPass == result.Password)
            {
                isValid = true;
            }

            user = new User(result);

            return isValid;
        }

        internal static string CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        internal static string GenerateSHA256Hash(string input, string salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);

            System.Security.Cryptography.SHA256Managed sha256hashstring = new System.Security.Cryptography.SHA256Managed();

            byte[] hash = sha256hashstring.ComputeHash(bytes);

            return ByteArrayToString(hash);
        }

        internal static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        
        internal static User GetUserByUsername(string username)
        {
            GetUserByUsername_Result resultSet = new GetUserByUsername_Result();       
            using(Data.WiseManEntities db = new Data.WiseManEntities())
            {
                resultSet = db.GetUserByUsername(username).FirstOrDefault();       
            }
            return new User(resultSet);
        }
    }
}