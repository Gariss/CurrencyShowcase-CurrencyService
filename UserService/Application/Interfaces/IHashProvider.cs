namespace UserService.Application.Interfaces;

public interface IHashProvider
{
    string Generate(string inputString);

    bool Check(string checkString, string hashString);
}