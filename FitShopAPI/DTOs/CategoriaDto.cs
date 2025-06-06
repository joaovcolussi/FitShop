namespace FitShopAPI.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }

    public class CreateCategoriaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }

    public class UpdateCategoriaDto
    {
        public string? Nome { get; set; }
        public string? Slug { get; set; }
    }
}