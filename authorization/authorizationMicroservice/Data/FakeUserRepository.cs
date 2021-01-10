using authorizationMicroservice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace authorizationMicroservice.Data
{
    public class FakeUserRepository:IFakeUserRepository
    {
        public static readonly List<User> users = new List<User>();
        public static readonly List<User> admins = new List<User>();
        public FakeUserRepository()
        {
            FillData();
        }
        public void FillData()
        {
            for (int i = 0; i < 5; i++)
            {
                var userPass = HashPassword($"userPass{i}");
                var adminPass = HashPassword($"adminPass{i}");
                users.Add(new User() { ID = Guid.NewGuid(), Username = $"UserUsername{i}", Password = userPass.Item1, Salt = userPass.Item2 });
                admins.Add(new User() { ID = Guid.NewGuid(), Username = $"AdminUsername{i}", Password = adminPass.Item1, Salt = adminPass.Item2 });
            }
        }
        private static Tuple<string, string> HashPassword(string password)
        {
            var sBytes = new byte[password.Length];
            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);
            var salt = Convert.ToBase64String(sBytes);

            var derivedBytes = new Rfc2898DeriveBytes(password, sBytes);

            return new Tuple<string, string>
            (
                Convert.ToBase64String(derivedBytes.GetBytes(256)),
                salt
            );
        }
    }
}
