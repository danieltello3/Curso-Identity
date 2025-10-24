using Galaxy.Security.Application.Dto;
using Galaxy.Security.Application.Dto.Users;

namespace Galaxy.Security.Application.InPorts.Users
{
    public interface ICreateUserUseCase
    {
        Task<IdentityResponse> ExecuteAsync(CreateUserRequest request);
    }
}
