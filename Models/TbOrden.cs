using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Models;

public partial class TbOrden
{
    public int IdOrden { get; set; }

    public decimal Total { get; set; }

    public string? Comentario { get; set; }

    public DateOnly FechaOrden { get; set; }

    public int? IdEstado { get; set; }

    public int IdUsuario { get; set; }

    public int? IdFlujo { get; set; }

    public virtual TbFlujoEstado? IdEstadoNavigation { get; set; }

    public virtual TbFlujoFinanciero? IdFlujoNavigation { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<TbArticulo> TbArticulos { get; set; } = new List<TbArticulo>();
}
