﻿using UserService.Domain.Entities;

namespace UserService.Application.Interfaces.Repository;

public interface IUserRepository
{
    Task<bool> AddAsync(User newUser, CancellationToken cancellationToken);

    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken);
}
