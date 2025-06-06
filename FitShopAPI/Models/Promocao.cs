using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitShopAPI.Models
{
    public class Promocao
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? Descricao { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Desconto { get; set; }
        
        public int ProdutoId { get; set; }
        
        // Navigation properties
        public virtual Produto Produto { get; set; } = null!;
    }
}