using System.Collections.Concurrent;

namespace ApiGatewayService.WebApi.Services;

public class WhitelistService : IWhitelistService
{
    private readonly ConcurrentDictionary<string, DateTime> _whiteList = new();
    private readonly ILogger<WhitelistService> _logger;

    public WhitelistService(ILogger<WhitelistService> logger)
    {
        _logger = logger;
    }

    public bool IsActive(string id)
    {
        return _whiteList.TryGetValue(id, out var expiry) && expiry > DateTime.UtcNow;
    }

    public void Add(string id, DateTime expiry)
    {
        if (!_whiteList.ContainsKey(id))
        {
            _whiteList[id] = expiry;
            _logger.LogInformation("Item {id} added to whitelist, valid until {expiry}", id, expiry);
        }
        else
        {
            _logger.LogWarning("Item {id} is already exists", id);
        }
    }

    public void Remove(string id)
    {
        if (_whiteList.TryRemove(id, out _))
        {
            _logger.LogInformation("Item {id} removed from whitelist", id);
        }
        else
        {
            _logger.LogWarning("Item {id} was not removed from whitelist", id);
        }
    }
}