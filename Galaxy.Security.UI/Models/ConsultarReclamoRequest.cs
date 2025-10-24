﻿using System.ComponentModel.DataAnnotations;

namespace Galaxy.Security.UI.Models
{
    public class ConsultarReclamoRequest
    {
        [Required(ErrorMessage = "El campo Codigo es obligatorio")]
        public string Codigo { get; set; }
        public GetReclamoResponse? Resultado { get; set; }
    }
}
