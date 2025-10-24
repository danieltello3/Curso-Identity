using System.ComponentModel.DataAnnotations;

namespace Galaxy.Security.Application.Dto.Reclamo
{
    public class CreateReclamoRequest
    {
        [Required(ErrorMessage = "El campo Descripcion es obligatorio")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        public DateTime Fecha { get; set; }
    }
}