using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Models;

public partial class TbFlujoFinanciero
{
    public int IdFlujoFinanciero { get; set; }

    public int IdRolAprobador { get; set; }

    public decimal MontoMinimo { get; set; }

    public decimal? MontoMaximo { get; set; }

    public virtual ICollection<TbOrden> TbOrdens { get; set; } = new List<TbOrden>();
}
