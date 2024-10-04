using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Models;

public partial class TbArticulo
{
    public int IdArticuloOrden { get; set; }

    public string Articulo { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public int IdOrden { get; set; }

    public virtual TbOrden IdOrdenNavigation { get; set; } = null!;
}
