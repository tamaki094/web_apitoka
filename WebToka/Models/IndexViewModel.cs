using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebToka.Models
{
    public class IndexViewModel: BaseModelo
    {
        public List<PersonaFisica> PersonasFisicas { get; set; }
    }
}