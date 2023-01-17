using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSalesSystem.Viewmodel
{
     public static class AccountInfo
    {
        private static string username;
        public static string Username { get { return username; } set { username = value; } }

        private static string idAccount;
        public static string IdAccount { get { return idAccount; } set { idAccount = value; } }
    }
}
