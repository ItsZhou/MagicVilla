using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_API.Modelos
{
    public class NumeroVilla
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        // Primary key, y DatabaseGenerated(DatabaseGeneratedOption.None) es un metodo que nos permite a nosotros asignar el ID y que no lo asigne automaticamente
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        [ForeignKey("VillaId")]
        public Villa Villa { get; set; }
        // Se crea esta propiedad de Villa para poder relacionar la tabla NumeroVilla con la tabla Villa
        public string DetalleEspecial { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
