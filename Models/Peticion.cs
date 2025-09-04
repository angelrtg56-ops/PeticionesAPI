using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeticionesAPI.Models
{
    [Table("Peticiones")] 
    public class Peticion
    {
        [Key]
        public int PeticionID { get; set; }

        public required string PeticionTexto { get; set; }

        public string? NombreOpcional { get; set; }

        // El tipo de dato es DateTime para coincidir con la base de datos
        public DateTime FechaCreacion { get; set; } 
    }
}