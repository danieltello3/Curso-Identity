using Galaxy.Security.Domain.Entities;
using Galaxy.Security.Domain.OutPort.Secrets;
using Galaxy.Security.Domain.OutPort.Services;
using Galaxy.Security.Infraestructure.Configurations.IdentityEntities;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Galaxy.Security.Infraestructure.Adapters.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserExtension> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokenStore _refreshToken;
        private readonly IVaultSecretsProvider _vaultService;
        public AuthService(
            IConfiguration configuration,
            UserManager<UserExtension> userManager,
            IHttpContextAccessor httpContextAccessor,
            IRefreshTokenStore refreshToken,
            IVaultSecretsProvider vaultSecretsProvider)
        {
            _configuration = configuration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _refreshToken = refreshToken;
            _vaultService = vaultSecretsProvider;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokenAsync(User userApp)
        {
            var secrets = _vaultService.GetSecretsAsync().GetAwaiter().GetResult();
            var secKey = secrets["JwtSecretKey"];
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secKey));
            var user = userApp.Adapt<UserExtension>();
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"])),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = GenerateSecureToken();
            var expiration = TimeSpan.FromHours(Convert.ToDouble(jwtSettings["RefreshTokenExpirationHours"]));

            //Se guardan en el almacén de RefreshTokens (Redis)
            await _refreshToken.SaveTokenAsync(user.Id, refreshToken, expiration);

            //Agregamos a loas Cookies el AccessToken y RefreshToken
            SetAuthCookies(accessToken, refreshToken);


            return (accessToken, refreshToken);
        }


        public async Task<(string AccessToken, string RefreshToken, User? User)> RefreshTokensAsync()
        {
            var context = _httpContextAccessor.HttpContext!;

            var refreshToken = context.Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException("Refresh token no encontrado");

            var userId = await _refreshToken.GetUserIdFromTokenAsync(refreshToken);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Refresh token inválido o expirado");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new UnauthorizedAccessException("Usuario no encontrado");

            var usuario = user.Adapt<User>();

            await _refreshToken.InvalidateTokenAsync(refreshToken);

            var tokens = await GenerateTokenAsync(usuario);

            return (tokens.AccessToken, tokens.RefreshToken, usuario);
        }

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[62];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public void RemoveAuthCookies()
        {
            var context = _httpContextAccessor.HttpContext!;

            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            context.Response.Cookies.Append("access_token", "", accessCookieOptions);
            context.Response.Cookies.Append("refresh_token", "", refreshCookieOptions);
        }
        private void SetAuthCookies(string accessToken, string refreshToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var context = _httpContextAccessor.HttpContext!;

            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"]))
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["RefreshTokenExpirationHours"]))
            };

            context.Response.Cookies.Append("access_token", accessToken, accessCookieOptions);
            context.Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
        }
    }
}
