using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess.Data
{
    public class Tarea
    {
        [Key]
        public int Id_Tarea { get; set; }

        [Required]
        public int Id_Meta { get; set; }

        [Required]
        public string Nombre { get; set; }
        public DateTime Fecha_Creacion { get; set; }

        [Required]
        public string Estado { get; set; }

        public bool Importante { get; set; }
    }
}
