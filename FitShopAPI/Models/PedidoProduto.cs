using System.ComponentModel.DataAnnotations;

namespace FitShopAPI.Models
{
    public class PedidoProduto
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        
        public int Quantidade { get; set; } = 1;
        
        // Navigation properties
        public virtual Pedido Pedido { get; set; } = null!;
        public virtual Produto Produto { get; set; } = null!;
    }
}