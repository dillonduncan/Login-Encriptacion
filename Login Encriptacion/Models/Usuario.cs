using System.ComponentModel.DataAnnotations;

namespace Login_Encriptacion.Models
{
    public class Usuario
    {
        [Key] //Primary Key
        public int Id { get; set; }

        [Required] 
        [MaxLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
