namespace ApiGatewayService.WebApi.Infrastructure;

public record CorsSettings
{
    public string[] AllowedOrigins { get; init; }
}