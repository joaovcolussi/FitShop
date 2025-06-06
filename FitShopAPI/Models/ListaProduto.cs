using System.ComponentModel.DataAnnotations;

namespace FitShopAPI.Models
{
    public class ListaProduto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? Descricao { get; set; }
    }
}