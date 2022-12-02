using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MetaModel
    {
        public int Id_Meta { get; set; }

        [Required(ErrorMessage = "Debe tener un nombre para darse de alta.")]
        [StringLength(80, ErrorMessage ="El nombre de la Meta no puede exceder de 80 caracteres.")]
        public string Nombre { get; set; }
        public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

        public int TotalTareas { get; set; }
        public int TareasCompletadas { get; set; }
        public string Porcentaje { get; set; }
    }
}
