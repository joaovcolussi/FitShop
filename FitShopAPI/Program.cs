using Microsoft.EntityFrameworkCore;
using FitShopAPI.Data;
using FitShopAPI.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MySQL
builder.Services.AddDbContext<FitShopContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVueApp");
app.UseAuthorization();
app.MapControllers();

// Seed inicial no banco (apenas se estiver vazio)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FitShopContext>();
    context.Database.EnsureCreated();

    if (!context.Produtos.Any())
    {
        context.Produtos.AddRange(new List<Produto>
        {
            new Produto
            {
                Nome = "Tênis Corrida Pro",
                Descricao = "Tênis leve e confortável para longas distâncias.",
                Preco = 229.90m,
                Categoria = "Calçados",
                Imagem = "https://via.placeholder.com/300x300.png?text=Tenis+Pro",
                Estoque = 30,
                Novidade = true,
                Promocao = true,
                PrecoPromocional = 199.90m
            },
            new Produto
            {
                Nome = "Camiseta Dry Performance",
                Descricao = "Tecnologia que elimina o suor rapidamente.",
                Preco = 89.90m,
                Categoria = "Roupas",
                Imagem = "https://via.placeholder.com/300x300.png?text=Camiseta",
                Estoque = 60,
                Novidade = true,
                Promocao = false
            },
            new Produto
            {
                Nome = "Smartwatch FitPulse",
                Descricao = "Monitora batimentos, passos e sono.",
                Preco = 349.90m,
                Categoria = "Eletrônicos",
                Imagem = "https://via.placeholder.com/300x300.png?text=Smartwatch",
                Estoque = 20,
                Novidade = true,
                Promocao = false
            },
            new Produto
            {
                Nome = "Whey Protein 900g",
                Descricao = "Suplemento proteico de alto valor biológico.",
                Preco = 129.90m,
                Categoria = "Suplementos",
                Imagem = "https://via.placeholder.com/300x300.png?text=Whey",
                Estoque = 40,
                Novidade = false,
                Promocao = true,
                PrecoPromocional = 109.90m
            },
            new Produto
            {
                Nome = "Shorts Treino Masculino",
                Descricao = "Confortável e com bolsos laterais.",
                Preco = 69.90m,
                Categoria = "Roupas",
                Imagem = "https://via.placeholder.com/300x300.png?text=Shorts",
                Estoque = 50,
                Novidade = false,
                Promocao = false
            },
            new Produto
            {
                Nome = "Legging Feminina Flex",
                Descricao = "Alta compressão e conforto extremo.",
                Preco = 99.90m,
                Categoria = "Roupas",
                Imagem = "https://via.placeholder.com/300x300.png?text=Legging",
                Estoque = 40,
                Novidade = true,
                Promocao = false
            },
            new Produto
            {
                Nome = "Creatina Monohidratada 300g",
                Descricao = "Melhora o desempenho e força.",
                Preco = 89.90m,
                Categoria = "Suplementos",
                Imagem = "https://via.placeholder.com/300x300.png?text=Creatina",
                Estoque = 25,
                Novidade = false,
                Promocao = false
            },
            new Produto
            {
                Nome = "Fone de Ouvido Bluetooth Sport",
                Descricao = "Resistente a suor e bateria de longa duração.",
                Preco = 149.90m,
                Categoria = "Eletrônicos",
                Imagem = "https://via.placeholder.com/300x300.png?text=Fone",
                Estoque = 35,
                Novidade = true,
                Promocao = false
            },
            new Produto
            {
                Nome = "Meia Esportiva Cano Médio (3 pares)",
                Descricao = "Confortável e resistente ao uso intenso.",
                Preco = 39.90m,
                Categoria = "Acessórios",
                Imagem = "https://via.placeholder.com/300x300.png?text=Meia",
                Estoque = 100,
                Novidade = false,
                Promocao = true,
                PrecoPromocional = 29.90m
            },
            new Produto
            {
                Nome = "Cinto de Musculação",
                Descricao = "Suporte lombar para levantamento de peso.",
                Preco = 79.90m,
                Categoria = "Acessórios",
                Imagem = "https://via.placeholder.com/300x300.png?text=Cinto",
                Estoque = 20,
                Novidade = false,
                Promocao = false
            }
        });

        context.SaveChanges();
    }
}

app.Run();
