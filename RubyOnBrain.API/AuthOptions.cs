using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RubyOnBrain.API
{
    public class AuthOptions
    {
        public const string ISSUER = "rubyonbrain.com"; // издатель токена
        public const string AUDIENCE = "rubyonbrainclient"; // потребитель токена
        const string KEY = "54eb405d5587bf6f0d4241aeb4e729d3e28c3b37fb1b50c8bca838e7142863bef3c4cd05dfec510908179260ad4238869b3b0b5692ff673358e31c833f252141";   // ключ для шифрации
        public const int LIFETIME = 44640;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)); // массив байтов, созданный по секретному ключу
    }
}
