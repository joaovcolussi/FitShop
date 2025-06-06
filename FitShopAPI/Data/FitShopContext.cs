using Microsoft.EntityFrameworkCore;
using FitShopAPI.Models;

namespace FitShopAPI.Data
{
    public class FitShopContext : DbContext
    {
        public FitShopContext(DbContextOptions<FitShopContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProduto> PedidoProdutos { get; set; }
        public DbSet<Promocao> Promocoes { get; set; }
        public DbSet<MaisComprado> MaisComprados { get; set; }
        public DbSet<MaisPesquisado> MaisPesquisados { get; set; }
        public DbSet<ListaProduto> ListasProdutos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for PedidoProduto
            modelBuilder.Entity<PedidoProduto>()
                .HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

            // Configure relationships
            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.PedidoProdutos)
                .HasForeignKey(pp => pp.PedidoId);

            modelBuilder.Entity<PedidoProduto>()
                .HasOne(pp => pp.Produto)
                .WithMany(p => p.PedidoProdutos)
                .HasForeignKey(pp => pp.ProdutoId);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId);

            modelBuilder.Entity<Promocao>()
                .HasOne(pr => pr.Produto)
                .WithMany(p => p.Promocoes)
                .HasForeignKey(pr => pr.ProdutoId);

            modelBuilder.Entity<MaisComprado>()
                .HasOne(mc => mc.Produto)
                .WithMany(p => p.MaisComprados)
                .HasForeignKey(mc => mc.ProdutoId);

            modelBuilder.Entity<MaisPesquisado>()
                .HasOne(mp => mp.Produto)
                .WithMany(p => p.MaisPesquisados)
                .HasForeignKey(mp => mp.ProdutoId);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categorias
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Suplementos", Slug = "suplementos" },
                new Categoria { Id = 2, Nome = "Acessórios", Slug = "acessorios" },
                new Categoria { Id = 3, Nome = "Roupas Fitness", Slug = "roupas" },
                new Categoria { Id = 4, Nome = "Equipamentos", Slug = "equipamentos" },
                new Categoria { Id = 5, Nome = "Nutrição", Slug = "nutricao" }
            );

            // Seed Produtos
            modelBuilder.Entity<Produto>().HasData(
                new Produto
                {
                    Id = 1,
                    Nome = "Whey Protein Gold Standard",
                    Descricao = "Whey Protein de alta qualidade com 24g de proteína por dose. Ideal para recuperação muscular e ganho de massa magra.",
                    Preco = 149.90m,
                    Categoria = "Suplementos",
                    Imagem = "https://images.pexels.com/photos/8844577/pexels-photo-8844577.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 50,
                    Novidade = true,
                    Promocao = true,
                    PrecoPromocional = 135.00m
                },
                new Produto
                {
                    Id = 2,
                    Nome = "Creatina Monohidratada 300g",
                    Descricao = "Creatina pura para aumento de força e resistência durante os treinos. 5g por dose diária.",
                    Preco = 89.90m,
                    Categoria = "Suplementos",
                    Imagem = "https://images.pexels.com/photos/12304205/pexels-photo-12304205.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 30,
                    Novidade = false,
                    Promocao = true,
                    PrecoPromocional = 79.90m
                },
                new Produto
                {
                    Id = 3,
                    Nome = "Luvas de Treino Premium",
                    Descricao = "Luvas de alta qualidade para proteção das mãos durante treinos de musculação. Material respirável.",
                    Preco = 59.90m,
                    Categoria = "Acessórios",
                    Imagem = "https://images.pexels.com/photos/6456264/pexels-photo-6456264.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 15,
                    Novidade = true,
                    Promocao = false
                },
                new Produto
                {
                    Id = 4,
                    Nome = "Camiseta Dry-Fit Performance",
                    Descricao = "Camiseta com tecnologia que absorve o suor e mantém o corpo seco durante a atividade física.",
                    Preco = 79.90m,
                    Categoria = "Roupas",
                    Imagem = "https://images.pexels.com/photos/6311251/pexels-photo-6311251.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 25,
                    Novidade = true,
                    Promocao = false
                },
                new Produto
                {
                    Id = 5,
                    Nome = "Barra de Proteína Crunchy",
                    Descricao = "Barra de proteína com 20g de proteína e baixo teor de açúcar. Perfeita para lanches entre refeições.",
                    Preco = 12.90m,
                    Categoria = "Suplementos",
                    Imagem = "https://images.pexels.com/photos/6249089/pexels-photo-6249089.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 100,
                    Novidade = false,
                    Promocao = true,
                    PrecoPromocional = 9.90m
                },
                new Produto
                {
                    Id = 6,
                    Nome = "Corda de Pular Profissional",
                    Descricao = "Corda de pular com cabo de aço revestido e rolamentos para alta velocidade.",
                    Preco = 45.90m,
                    Categoria = "Acessórios",
                    Imagem = "https://images.pexels.com/photos/4473608/pexels-photo-4473608.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 20,
                    Novidade = true,
                    Promocao = false
                },
                new Produto
                {
                    Id = 7,
                    Nome = "Legging Compressão Feminina",
                    Descricao = "Legging de compressão para mulheres com tecido respirável e cintura alta.",
                    Preco = 89.90m,
                    Categoria = "Roupas",
                    Imagem = "https://images.pexels.com/photos/6550839/pexels-photo-6550839.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 30,
                    Novidade = true,
                    Promocao = true,
                    PrecoPromocional = 75.00m
                },
                new Produto
                {
                    Id = 8,
                    Nome = "BCAA 2:1:1 - 60 cápsulas",
                    Descricao = "Suplemento de aminoácidos para preservação muscular e melhor recuperação.",
                    Preco = 69.90m,
                    Categoria = "Suplementos",
                    Imagem = "https://images.pexels.com/photos/4464819/pexels-photo-4464819.jpeg?auto=compress&cs=tinysrgb&w=600",
                    Estoque = 40,
                    Novidade = false,
                    Promocao = false
                }
            );
        }
    }
}