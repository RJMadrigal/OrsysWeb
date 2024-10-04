namespace SistemaOrdenes.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = null!;

        public string NombreUsuario { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Clave { get; set; } = null!;

        public bool? Restablecer { get; set; }

        public bool? Confirmado { get; set; }

        public string? Token { get; set; }

        public int IdRol { get; set; }

        public int? IdJefe { get; set; }
    }
}
