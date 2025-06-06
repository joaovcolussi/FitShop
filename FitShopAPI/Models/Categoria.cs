using System.ComponentModel.DataAnnotations;

namespace FitShopAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Slug { get; set; } = string.Empty;
    }
}