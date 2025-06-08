using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitShopAPI.Data;
using FitShopAPI.Models;
using FitShopAPI.DTOs;

namespace FitShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly FitShopContext _context;

        public ProdutosController(FitShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos(
            [FromQuery] string? termo,
            [FromQuery] string? categoria)
        {
            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(termo))
            {
                query = query.Where(p => p.Nome.Contains(termo) || 
                                        (p.Descricao != null && p.Descricao.Contains(termo)));
                
                // Registrar pesquisa
                await RegistrarPesquisa(termo);
            }

            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(p => p.Categoria != null && 
                                        p.Categoria.ToLower() == categoria.ToLower());
            }

            var produtos = await query.Select(p => new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Categoria = p.Categoria,
                Imagem = p.Imagem,
                Estoque = p.Estoque,
                Novidade = p.Novidade,
                Promocao = p.Promocao,
                PrecoPromocional = p.PrecoPromocional
            }).ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            var produtoDto = new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria,
                Imagem = produto.Imagem,
                Estoque = produto.Estoque,
                Novidade = produto.Novidade,
                Promocao = produto.Promocao,
                PrecoPromocional = produto.PrecoPromocional
            };

            return Ok(produtoDto);
        }

        [HttpGet("promocoes")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetPromocoes()
        {
            var produtos = await _context.Produtos
                .Where(p => p.Promocao)
                .Select(p => new ProdutoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    Preco = p.Preco,
                    Categoria = p.Categoria,
                    Imagem = p.Imagem,
                    Estoque = p.Estoque,
                    Novidade = p.Novidade,
                    Promocao = p.Promocao,
                    PrecoPromocional = p.PrecoPromocional
                }).ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("novidades")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetNovidades()
        {
            var produtos = await _context.Produtos
                .Where(p => p.Novidade)
                .Select(p => new ProdutoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    Preco = p.Preco,
                    Categoria = p.Categoria,
                    Imagem = p.Imagem,
                    Estoque = p.Estoque,
                    Novidade = p.Novidade,
                    Promocao = p.Promocao,
                    PrecoPromocional = p.PrecoPromocional
                }).ToListAsync();

            return Ok(produtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> CreateProduto(CreateProdutoDto createDto)
        {
            var produto = new Produto
            {
                Nome = createDto.Nome,
                Descricao = createDto.Descricao,
                Preco = createDto.Preco,
                Categoria = createDto.Categoria,
                Imagem = createDto.Imagem,
                Estoque = createDto.Estoque,
                Novidade = createDto.Novidade,
                Promocao = createDto.Promocao,
                PrecoPromocional = createDto.PrecoPromocional
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var produtoDto = new ProdutoDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria,
                Imagem = produto.Imagem,
                Estoque = produto.Estoque,
                Novidade = produto.Novidade,
                Promocao = produto.Promocao,
                PrecoPromocional = produto.PrecoPromocional
            };

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produtoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduto(int id, UpdateProdutoDto updateDto)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            if (updateDto.Nome != null) produto.Nome = updateDto.Nome;
            if (updateDto.Descricao != null) produto.Descricao = updateDto.Descricao;
            if (updateDto.Preco.HasValue) produto.Preco = updateDto.Preco.Value;
            if (updateDto.Categoria != null) produto.Categoria = updateDto.Categoria;
            if (updateDto.Imagem != null) produto.Imagem = updateDto.Imagem;
            if (updateDto.Estoque.HasValue) produto.Estoque = updateDto.Estoque.Value;
            if (updateDto.Novidade.HasValue) produto.Novidade = updateDto.Novidade.Value;
            if (updateDto.Promocao.HasValue) produto.Promocao = updateDto.Promocao.Value;
            if (updateDto.PrecoPromocional.HasValue) produto.PrecoPromocional = updateDto.PrecoPromocional.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

        private async Task RegistrarPesquisa(string termo)
        {
            // Buscar produtos que correspondem ao termo
            var produtos = await _context.Produtos
                .Where(p => p.Nome.Contains(termo) || 
                           (p.Descricao != null && p.Descricao.Contains(termo)))
                .ToListAsync();

            foreach (var produto in produtos)
            {
                var maisPesquisado = await _context.MaisPesquisados
                    .FirstOrDefaultAsync(mp => mp.ProdutoId == produto.Id);

                if (maisPesquisado == null)
                {
                    maisPesquisado = new MaisPesquisado
                    {
                        ProdutoId = produto.Id,
                        TotalPesquisas = 1
                    };
                    _context.MaisPesquisados.Add(maisPesquisado);
                }
                else
                {
                    maisPesquisado.TotalPesquisas++;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}