namespace FitShopAPI.DTOs
{
    public class WhatsAppPedidoDto
    {
        public List<WhatsAppItemDto> Items { get; set; } = new List<WhatsAppItemDto>();
        public decimal Total { get; set; }
        public string Telefone { get; set; } = string.Empty;
    }

    public class WhatsAppItemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public string? Imagem { get; set; }
    }

    public class WhatsAppResponseDto
    {
        public string Url { get; set; } = string.Empty;
    }
}