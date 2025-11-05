using UserService.Application.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Application.Contracts.Requests;
using UserService.Application.Contracts.Responses;
using Microsoft.Extensions.Options;
using UserService.Application.Infrastructure;

namespace UserService.Application.Services;

public class UserService(
    IUserRepository userRepository,
    ILogger<UserService> logger,
    IHashProvider hashProvider,
    IJwtProvider jwtProvider,
    IOptions<JwtOptions> jwtOptions) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<UserService> _logger = logger;
    private readonly IHashProvider _hashProvider = hashProvider;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly JwtOptions _jwtOptions = jwtOptions?.Value
        ?? throw new ArgumentNullException(nameof(JwtOptions));

    public async Task RegisterAsync(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("The user is already exists!");
        }

        var newUser = new User
        {
            Name = request.Name,
            Email = request.Email,
            Login = request.Login,
            PasswordHash = _hashProvider.Generate(request.Password)
        };

        var result = await _userRepository.AddAsync(newUser, cancellationToken);
        
        if(result)
        {
            _logger.LogInformation("New user registered: {@request}", request);
        }
        else
        {
            throw new Exception($"Unable to register user {request.Login}");
        }
    }

    public async Task<UserResponse?> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLoginAsync(login, cancellationToken);

        return user is not null ?
            new UserResponse
            {
                Name = user.Name,
                Email = user.Email,
                Login = user.Login,
                RegistrationDate = user.CreatedAt
            } :
            null;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to login: {@request}", request);

        var user = await _userRepository.GetByLoginAsync(request.Login, cancellationToken);
        bool isPasswordVerified = false;

        if (user != null)
        {
            isPasswordVerified = _hashProvider.Check(request.Password, user.PasswordHash); 
        }

        if(user is null || !isPasswordVerified)
        {
            throw new UnauthorizedAccessException("Wrong user login or password");
        }

        var token = _jwtProvider.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(_jwtOptions.ExpiryHours);

        var response = new LoginResponse
        {
            Token = token,
            ExpiresAt = expiresAt
        };

        _logger.LogInformation("The user has logged in: {@request}", request);

        return response;
    }
}