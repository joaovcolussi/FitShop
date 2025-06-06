using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitShopAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        
        public int ClienteId { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Data { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorTotal { get; set; }
        
        // Navigation properties
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();
    }
}