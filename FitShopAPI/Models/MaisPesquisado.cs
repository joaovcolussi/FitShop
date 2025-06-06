namespace FitShopAPI.Models
{
    public class MaisPesquisado
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int TotalPesquisas { get; set; } = 0;
        
        // Navigation properties
        public virtual Produto Produto { get; set; } = null!;
    }
}