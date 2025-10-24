using Galaxy.Security.Domain.Exceptions;

namespace Galaxy.Security.Domain.Entities
{
    public class Reclamo
    {
        public Guid Id { get; private set; }
        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public DateTime Fecha { get; private set; }        
        public Reclamo()
        {
        }

        public Reclamo(string codigo, string descripcion, DateTime fecha)
        {
            Id = Guid.NewGuid();
            Codigo = codigo;
            Descripcion = descripcion;
            Fecha = fecha;
        }

        public static Reclamo Create(string codigo, string descripcion, DateTime fecha)
        {
            if (string.IsNullOrEmpty(codigo)) throw new DomainException("codigo is required");
            if (string.IsNullOrEmpty(descripcion)) throw new DomainException("descripcion is required");

            return new Reclamo(codigo, descripcion, fecha);
        }

    }
}
