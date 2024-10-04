using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Models;

public partial class TbRole
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Usuarios> TbUsuarios { get; set; } = new List<Usuarios>();
}
