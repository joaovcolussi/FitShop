using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitShopAPI.Data;
using FitShopAPI.Models;
using FitShopAPI.DTOs;

namespace FitShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly FitShopContext _context;

        public PedidosController(FitShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
                .Select(p => new PedidoDto
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    Data = p.Data,
                    ValorTotal = p.ValorTotal,
                    Produtos = p.PedidoProdutos.Select(pp => new PedidoProdutoDto
                    {
                        ProdutoId = pp.ProdutoId,
                        NomeProduto = pp.Produto.Nome,
                        Quantidade = pp.Quantidade,
                        Preco = pp.Produto.Preco
                    }).ToList()
                }).ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDto>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                ClienteId = pedido.ClienteId,
                Data = pedido.Data,
                ValorTotal = pedido.ValorTotal,
                Produtos = pedido.PedidoProdutos.Select(pp => new PedidoProdutoDto
                {
                    ProdutoId = pp.ProdutoId,
                    NomeProduto = pp.Produto.Nome,
                    Quantidade = pp.Quantidade,
                    Preco = pp.Produto.Preco
                }).ToList()
            };

            return Ok(pedidoDto);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoDto>> CreatePedido(CreatePedidoDto createDto)
        {
            var pedido = new Pedido
            {
                ClienteId = createDto.ClienteId,
                Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ValorTotal = createDto.ValorTotal
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Adicionar produtos ao pedido
            foreach (var produtoDto in createDto.Produtos)
            {
                var pedidoProduto = new PedidoProduto
                {
                    PedidoId = pedido.Id,
                    ProdutoId = produtoDto.ProdutoId,
                    Quantidade = produtoDto.Quantidade
                };

                _context.PedidoProdutos.Add(pedidoProduto);
            }

            await _context.SaveChangesAsync();

            // Carregar o pedido completo para retornar
            var pedidoCompleto = await _context.Pedidos
                .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(p => p.Id == pedido.Id);

            var pedidoDto = new PedidoDto
            {
                Id = pedidoCompleto!.Id,
                ClienteId = pedidoCompleto.ClienteId,
                Data = pedidoCompleto.Data,
                ValorTotal = pedidoCompleto.ValorTotal,
                Produtos = pedidoCompleto.PedidoProdutos.Select(pp => new PedidoProdutoDto
                {
                    ProdutoId = pp.ProdutoId,
                    NomeProduto = pp.Produto.Nome,
                    Quantidade = pp.Quantidade,
                    Preco = pp.Produto.Preco
                }).ToList()
            };

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedidoDto);
        }
    }
}