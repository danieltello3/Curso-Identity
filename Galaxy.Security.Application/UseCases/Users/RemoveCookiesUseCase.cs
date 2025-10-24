﻿using Galaxy.Security.Application.InPorts.Users;
using Galaxy.Security.Domain.OutPort.Services;

namespace Galaxy.Security.Application.UseCases.Users
{
    public class RemoveCookiesUseCase : IRemoveCookiesUseCase
    {
        private readonly IAuthService _authService;
        public RemoveCookiesUseCase(IAuthService authService)
        {
            _authService = authService;
        }

        public string ExecuteAsync()
        {
            _authService.RemoveAuthCookies();
            return "Sesión cerrada exitosamente";
        }
    }
}
