using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    /// <summary>
    /// Representa un usuario en el sistema.
    /// </summary>
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(15)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!; 

        [Required]
        [MaxLength(30)]
        public string Password { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;
    }
}
