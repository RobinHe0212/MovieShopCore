using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MovieShop.Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {
        public string GetSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        public string HashingPassword(string psw, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                                      password: psw,
                                                                      salt: Convert.FromBase64String(salt),
                                                                      prf: KeyDerivationPrf.HMACSHA512,
                                                                      iterationCount: 10000,
                                                                      numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
