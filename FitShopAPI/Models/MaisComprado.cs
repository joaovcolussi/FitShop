namespace FitShopAPI.Models
{
    public class MaisComprado
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int TotalCompras { get; set; } = 0;
        
        // Navigation properties
        public virtual Produto Produto { get; set; } = null!;
    }
}