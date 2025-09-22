using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Callcenter.Api.Services;

public class MyPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    public string HashPassword(TUser user, string password)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] checkSum = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        var hash = BitConverter.ToString(checkSum).Replace("-","");
        return hash;
    }

    public PasswordVerificationResult VerifyHashedPassword(
        TUser user, string hashedPassword, string providedPassword)
    {
        var providedHash = HashPassword(user, providedPassword);

        if (hashedPassword == providedHash)
            return PasswordVerificationResult.Success;

        return PasswordVerificationResult.Failed;
    }
}