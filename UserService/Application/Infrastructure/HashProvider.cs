using UserService.Application.Interfaces;

namespace UserService.Application.Infrastructure;

/// <summary>
/// Hash provider
/// </summary>
public class HashProvider : IHashProvider
{
    public string Generate(string inputString)
    {
        return BCrypt.Net.BCrypt.HashPassword(inputString);
    }

    public bool Check(string checkString, string hashString)
    {
        return BCrypt.Net.BCrypt.Verify(checkString, hashString);
    }
}