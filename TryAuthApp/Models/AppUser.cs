using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TryAuthApp.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public int IsActive { get; set; } = 1; // par défaut c'est activé
        [Required]
        [MaxLength(30)]
        public string? Profil { get; set; }
        [MaxLength(200)]
        public string? AdresseUtilisateur { get; set; }
    }
}
