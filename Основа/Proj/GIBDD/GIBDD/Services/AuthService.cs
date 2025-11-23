using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIBDD.Models;
using System.Threading.Tasks;

namespace GIBDD.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthService
    {
        public static Adm CurrentUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            var user = new DataService<Adm>().GetAll()
                .FirstOrDefault(u => u.Login == username);

            if (user != null && password == user.Password)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        //public bool HasAccess(string requiredRole)
        //    => CurrentUser?.Role == requiredRole;
    }
}
