using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.Data
{
    public class Meta
    {
        [Key]
        public int Id_Meta { get; set; }

        [Required]
        public string Nombre { get; set; }
        public DateTime Fecha_Creacion { get; set; }
    }
}
