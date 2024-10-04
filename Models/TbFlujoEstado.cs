using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Models;

public partial class TbFlujoEstado
{
    public int IdFlujoEstado { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<TbOrden> TbOrdens { get; set; } = new List<TbOrden>();
}
