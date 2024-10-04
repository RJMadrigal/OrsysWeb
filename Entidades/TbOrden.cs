using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Entidades;

public partial class TbOrden
{
    public int IdOrden { get; set; }

    public string NombreArticulo { get; set; } = null!;

    public string Modelo { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public string Detalles { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public int IdUsuarioComprador { get; set; }

    public decimal? Total { get; set; }

    public virtual TbUsuario IdUsuarioCompradorNavigation { get; set; } = null!;

    public virtual ICollection<TbHistorial> TbHistorials { get; set; } = new List<TbHistorial>();
}
