using SD;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class TareaModel
    {
        public int Id_Tarea { get; set; }

        [Required]
        public int Id_Meta { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "El nombre de la tarea no puede exceder de 80 caracteres.")]
        public string Nombre { get; set; }
        public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

        [Required]
        public string Estado { get; set; } = SD.SD.Estatus_Abierta;

        public bool Importante { get; set; } = false;
    }
}
