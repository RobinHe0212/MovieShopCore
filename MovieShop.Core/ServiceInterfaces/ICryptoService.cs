using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Core.ServiceInterfaces
{
   public interface ICryptoService
    {
       string GetSalt();
        string HashingPassword(string psw, string salt);
    }
}
