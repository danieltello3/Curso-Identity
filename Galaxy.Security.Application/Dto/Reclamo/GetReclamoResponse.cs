﻿namespace Galaxy.Security.Application.Dto.Reclamo
{
    public class GetReclamoResponse
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}