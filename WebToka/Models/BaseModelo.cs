using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebToka.Models
{
    public class BaseModelo
    {
        public int PaginaActual { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistroPorPagina { get; set; }
    }
}