using Galaxy.Security.Application.Dto;
using Galaxy.Security.Application.Dto.Users;
using Galaxy.Security.Application.InPorts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Galaxy.Security.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICreateUserUseCase _useCase;
        private readonly ILoginUseCase _useLogin;
        private readonly IRemoveCookiesUseCase _useRemoveCookies;
        private readonly IRefreshTokenUseCase _useRefresh;
        public AuthController(ICreateUserUseCase useCase, ILoginUseCase useLogin, IRefreshTokenUseCase useRefresh, IRemoveCookiesUseCase useRemoveCookies)
        {
            _useCase = useCase;
            _useLogin = useLogin;
            _useRefresh = useRefresh;
            _useRemoveCookies = useRemoveCookies;
        }

        [HttpPost("CreateUser")]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _useCase.ExecuteAsync(request);
            return Ok(BaseResponse<IdentityResponse>.Success(result));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _useLogin.ExecuteAsync(request);
            return Ok(BaseResponse<string>.Success(result));
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            var result = _useRemoveCookies.ExecuteAsync();
            return Ok(BaseResponse<string>.Success(result));
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            await _useRefresh.ExecuteAsync();
            return Ok(BaseResponse<string>.Success("Token refrescado exitosamente."));
        }

        [HttpGet("Me")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated) return Unauthorized();

            var claims = identity.Claims.Select(c => new { c.Type, c.Value });

            return Ok(BaseResponse<object>.Success(claims));
        }
    }
}
