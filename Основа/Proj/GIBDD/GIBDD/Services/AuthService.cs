using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIBDD.Models;
using System.Threading.Tasks;

namespace GIBDD.Services
{
    /// <summary>
    /// Сервис для аутентификации пользователей
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public static Adm CurrentUser { get; set; }

        /// <summary>
        /// Выполняет вход пользователя в систему
        /// </summary>
        /// <param name="username">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>True, если вход выполнен успешно, иначе False</returns>
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
