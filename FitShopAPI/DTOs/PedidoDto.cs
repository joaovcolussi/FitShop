namespace FitShopAPI.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Data { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public List<PedidoProdutoDto> Produtos { get; set; } = new List<PedidoProdutoDto>();
    }

    public class CreatePedidoDto
    {
        public int ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CreatePedidoProdutoDto> Produtos { get; set; } = new List<CreatePedidoProdutoDto>();
    }

    public class PedidoProdutoDto
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }

    public class CreatePedidoProdutoDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}