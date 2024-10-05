using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Models;

public partial class TbHistorial
{
    public int IdHistorial { get; set; }

    public int IdOrden { get; set; }

    public int IdUsuario { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaActualizacion { get; set; }

    public string Comentarios { get; set; } = null!;

    public virtual TbOrden IdOrdenNavigation { get; set; } = null!;

    public virtual TbUsuario IdUsuarioNavigation { get; set; } = null!;
}
