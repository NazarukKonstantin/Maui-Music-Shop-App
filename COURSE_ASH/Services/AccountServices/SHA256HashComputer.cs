using System.Security.Cryptography;
using System.Text;

namespace COURSE_ASH.Services.AccountServices;

public static class SHA256HashComputer
{
    public static string ComputeSha256Hash(string rawData)
    {
        // Create a SHA256   
        using SHA256 sha256Hash = SHA256.Create();
        // ComputeHash - returns byte array  
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        // Convert byte array to a string
        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2")); //hexademical format
        }
        return builder.ToString();
    }
}
