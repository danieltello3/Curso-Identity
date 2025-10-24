using Galaxy.Security.Application.Dto;
using Galaxy.Security.Application.Dto.Users;
using Galaxy.Security.Application.InPorts.Users;
using Galaxy.Security.Domain.Dpo;
using Galaxy.Security.Domain.Entities;
using Galaxy.Security.Domain.OutPort.Persistence;
using Galaxy.Security.Domain.OutPort.Services;

namespace Galaxy.Security.Application.UseCases.Users
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailApiService _emailApiService;
        public CreateUserUseCase(IUserRepository userRepository, IEmailApiService emailApiService)
        {
            _userRepository = userRepository;
            _emailApiService = emailApiService;
        }

        public async Task<IdentityResponse> ExecuteAsync(CreateUserRequest request)
        {
            var user = User.Create(new Guid(), request.FullName, request.UserName, request.Email, request.Password);
            var result = await _userRepository.CreateUserAsync(user, request.Role);
            if (!result.Success) throw new ApplicationException(string.Join(", ", result.Errors));

            var email = new SendEmailDpo
            {
                Recipients = request.Email,
                Subject = "Welcome to Galaxy",
                Body = $"<h1>Hola {request.FullName} </h1>" +
                    $"<p>Tu cuenta fue creado exitosamente.</p> <br>" +
                    $"<b>Usuario: {request.UserName} <br>" +
                    $"Por valor haz clic sobre este enlace para confirmar tu correo electronico<br>" +
                    $"Muchas gracias, <b>Galaxy</b>"
            };

            await _emailApiService.SendEmailAsync(email);

            return new IdentityResponse
            {
                Success = result.Success,
                Errors = result.Errors
            };
        }
    }
}
