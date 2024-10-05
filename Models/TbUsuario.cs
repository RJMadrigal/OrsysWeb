using System;
using System.Collections.Generic;

namespace SistemaOrdenes.Models;

public partial class TbUsuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public bool? Restablecer { get; set; }

    public bool? Confirmado { get; set; }

    public string? Token { get; set; }

    public int IdRol { get; set; }

    public int? IdJefe { get; set; }

    public virtual TbUsuario? IdJefeNavigation { get; set; }

    public virtual TbRole IdRolNavigation { get; set; } = null!;

    public virtual ICollection<TbUsuario> InverseIdJefeNavigation { get; set; } = new List<TbUsuario>();

    public virtual ICollection<TbHistorial> TbHistorials { get; set; } = new List<TbHistorial>();

    public virtual ICollection<TbOrden> TbOrdens { get; set; } = new List<TbOrden>();
}
