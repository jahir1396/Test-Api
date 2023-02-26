using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Proyecto
{
    [Table("CatParametro")]
    public partial class CatParametro
    {
        [Key,Column("IdParametro")]
        public string IdParametro { get; set; }

        public string ValorParametro { get; set; }

    }
}
