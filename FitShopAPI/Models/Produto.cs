using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitShopAPI.Models
{
    public class Produto
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }
        
        [StringLength(1000)]
        public string? Descricao { get; set; }
        
        [StringLength(255)]
        public string? Categoria { get; set; }
        
        [StringLength(500)]
        public string? Imagem { get; set; }
        
        public int Estoque { get; set; } = 0;
        
        public bool Novidade { get; set; } = false;
        
        public bool Promocao { get; set; } = false;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecoPromocional { get; set; }
        
        // Navigation properties
        public virtual ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();
        public virtual ICollection<Promocao> Promocoes { get; set; } = new List<Promocao>();
        public virtual ICollection<MaisComprado> MaisComprados { get; set; } = new List<MaisComprado>();
        public virtual ICollection<MaisPesquisado> MaisPesquisados { get; set; } = new List<MaisPesquisado>();
    }
}