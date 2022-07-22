using System;
using System.Collections.Generic;

namespace BackApi_JWT.Models
{
    public partial class Producto
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public decimal? Precio { get; set; }
    }
}
