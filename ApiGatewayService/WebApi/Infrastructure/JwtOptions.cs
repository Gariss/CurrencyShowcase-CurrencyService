namespace ApiGatewayService.WebApi.Infrastructure;

public record JwtOptions
{
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int ExpiryHours { get; init; }
}
