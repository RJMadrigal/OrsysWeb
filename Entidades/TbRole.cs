using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Entidades;

public partial class TbRole
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public decimal? MontoMaximo { get; set; }

    public decimal? MontoMinimo { get; set; }

    public virtual ICollection<TbUsuario> TbUsuarios { get; set; } = new List<TbUsuario>();
}
