namespace ApiGatewayService.WebApi.Services;

public interface IWhitelistService
{
    bool IsActive(string id);
    void Add(string id, DateTime expiry);
    void Remove(string id);
}
