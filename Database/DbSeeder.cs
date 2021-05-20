using surer_backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace surer_backend.Database
{
    public class DbSeeder
    {
        public DbSeeder(SurerContext dbcontext)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                User user1 = new User();
                user1.Id = Guid.NewGuid().ToString();
                user1.FirstName = "nyein";
                user1.LastName = "wai";
                user1.Email = "nhw@gmail.com";
                user1.Password = MD5Hash.GetMd5Hash(md5Hash, "123");
                user1.ContactNumber = 1234;
                dbcontext.Add(user1);
                dbcontext.SaveChanges();
            }
        }
    }
}
