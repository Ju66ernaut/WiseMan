using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiseMan.API.Models;

namespace WiseMan.API.Utility
{
    public static class AccountHelper
    {
        //public HttpResponseMessage hash()
        //{
        //    //salt needs to be per-user, per-password
        //    //never reuse salt
        //    //store salt alongside hash
        //    //to store a pass:
        //    ////Generate a long random salt using a CSPRNG.
        //    ////Prepend the salt to the password and hash it with a standard cryptographic hash function such as SHA256.
        //    ////Save both the salt and the hash in the user's database record.

        //    //to validate
        //    ////Retrieve the user's salt and hash from the database.
        //    ////Prepend the salt to the given password and hash it using the same hash function.
        //    ////Compare the hash of the given password with the hash from the database.If they match, the password is correct.Otherwise, the password is incorrect.

        //    string strSalt = "7QsfeUxIvWjSOw==";
        //    string checkHashedPass = GenerateSHA256Hash("password1", strSalt);

        //    string salt = CreateSalt(10);
        //    string hashedPass = GenerateSHA256Hash("password1", salt);
        //    return new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StringContent("salt = " + salt + " --- hash = " + hashedPass)
        //    };
        //}
        internal static void RegisterNewAccount(string username, string password, string email)
        {            

            int size = new Random().Next(10, 20);
            string salt = CreateSalt(size);
            string hashedPass = GenerateSHA256Hash(password, salt);
            try
            {
                using(Data.WiseManEntities db = new Data.WiseManEntities())
                {
                    db.RegisterNewAccount(username, password, salt, email);
                };
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //TODO
        internal static bool ValidateUser(string username, string password, out User user)
        {
            throw new NotImplementedException();
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

        //TODO
        internal static User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}