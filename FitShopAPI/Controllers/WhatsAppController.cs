using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitShopAPI.Data;
using FitShopAPI.Models;
using FitShopAPI.DTOs;
using System.Text;
using System.Web;

namespace FitShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        private readonly FitShopContext _context;

        public WhatsAppController(FitShopContext context)
        {
            _context = context;
        }

        [HttpPost("enviar-pedido")]
        public async Task<ActionResult<WhatsAppResponseDto>> EnviarPedidoWhatsApp(WhatsAppPedidoDto pedidoDto)
        {
            try
            {
                // Registrar compras para estatísticas
                await RegistrarCompras(pedidoDto.Items);

                // Construir mensagem do WhatsApp
                var mensagem = new StringBuilder();
                mensagem.AppendLine("Olá! A loja FitShop recebeu seu pedido, gostaria de seguir com ele:");
                mensagem.AppendLine();

                foreach (var item in pedidoDto.Items)
                {
                    mensagem.AppendLine($"*{item.Nome}*");
                    mensagem.AppendLine($"Quantidade: {item.Quantidade}");
                    mensagem.AppendLine($"Preço unitário: {item.Preco:C}");
                    mensagem.AppendLine($"Subtotal: {(item.Preco * item.Quantidade):C}");
                    mensagem.AppendLine();
                }

                mensagem.AppendLine($"*Total do pedido: {pedidoDto.Total:C}*");
                mensagem.AppendLine();

                // Limpar e formatar telefone
                var numeroFormatado = new string(pedidoDto.Telefone.Where(char.IsDigit).ToArray());
                
                // Construir URL do WhatsApp
                var mensagemCodificada = HttpUtility.UrlEncode(mensagem.ToString());
                var url = $"https://wa.me/{numeroFormatado}?text={mensagemCodificada}";

                var response = new WhatsAppResponseDto
                {
                    Url = url
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao processar pedido: {ex.Message}");
            }
        }

        private async Task RegistrarCompras(List<WhatsAppItemDto> items)
        {
            foreach (var item in items)
            {
                var maisComprado = await _context.MaisComprados
                    .FirstOrDefaultAsync(mc => mc.ProdutoId == item.Id);

                if (maisComprado == null)
                {
                    maisComprado = new MaisComprado
                    {
                        ProdutoId = item.Id,
                        TotalCompras = item.Quantidade
                    };
                    _context.MaisComprados.Add(maisComprado);
                }
                else
                {
                    maisComprado.TotalCompras += item.Quantidade;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}