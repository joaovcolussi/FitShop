using System.ComponentModel.DataAnnotations;

namespace FitShopAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string CPF { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? Email { get; set; }
        
        // Navigation properties
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}