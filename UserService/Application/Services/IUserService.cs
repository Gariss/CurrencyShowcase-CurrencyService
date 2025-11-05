using UserService.Application.Contracts.Requests;
using UserService.Application.Contracts.Responses;

namespace UserService.Application.Services;

public interface IUserService
{
    Task RegisterAsync(UserRegisterRequest request, CancellationToken cancellationToken);

    Task<UserResponse?> GetByLoginAsync(string login, CancellationToken cancellationToken);

    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
