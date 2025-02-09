﻿using NuGet.Common;
using System.Text;

namespace ADOPSEV1._1
{
    public class HashFun
    {
        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                   System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value))
            );

        }
    }
}
