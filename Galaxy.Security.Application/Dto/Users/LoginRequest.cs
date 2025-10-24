using System.ComponentModel.DataAnnotations;

namespace Galaxy.Security.Application.Dto.Users
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El campo Usuario es obligatorio")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string Password { get; set; } = default!;
    }
}
