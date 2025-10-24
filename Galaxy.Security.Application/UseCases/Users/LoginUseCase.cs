﻿using Galaxy.Security.Application.Dto.Users;
using Galaxy.Security.Application.InPorts.Users;
using Galaxy.Security.Domain.OutPort.Persistence;
using Galaxy.Security.Domain.OutPort.Services;

namespace Galaxy.Security.Application.UseCases.Users
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        public LoginUseCase(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<string> ExecuteAsync(LoginRequest request)
        {
            var user = await _userRepository.FindByUserNameAsync(request.UserName);
            if (user is null)
                throw new UnauthorizedAccessException("Usuario no encontrado.");

            var password = await _userRepository.CheckPasswordAsync(user, request.Password);
            if (!password)
                throw new UnauthorizedAccessException("Contraseña incorrecta.");

            var token = await _authService.GenerateTokenAsync(user);

            return "Sesión iniciada correctamente.";
        }
    }
}
