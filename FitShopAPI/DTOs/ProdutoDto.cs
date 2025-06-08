namespace FitShopAPI.DTOs
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? Categoria { get; set; }
        public string? Imagem { get; set; }
        public int Estoque { get; set; }
        public bool Novidade { get; set; }
        public bool Promocao { get; set; }
        public decimal? PrecoPromocional { get; set; }
    }

    public class CreateProdutoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? Categoria { get; set; }
        public string? Imagem { get; set; }
        public int Estoque { get; set; }
        public bool Novidade { get; set; }
        public bool Promocao { get; set; }
        public decimal? PrecoPromocional { get; set; }
    }

    public class UpdateProdutoDto
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal? Preco { get; set; }
        public string? Categoria { get; set; }
        public string? Imagem { get; set; }
        public int? Estoque { get; set; }
        public bool? Novidade { get; set; }
        public bool? Promocao { get; set; }
        public decimal? PrecoPromocional { get; set; }
    }
}