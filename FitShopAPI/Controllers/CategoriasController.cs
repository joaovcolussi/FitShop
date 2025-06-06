using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitShopAPI.Data;
using FitShopAPI.Models;
using FitShopAPI.DTOs;

namespace FitShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly FitShopContext _context;

        public CategoriasController(FitShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Slug = c.Slug
                }).ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = new CategoriaDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Slug = categoria.Slug
            };

            return Ok(categoriaDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> CreateCategoria(CreateCategoriaDto createDto)
        {
            var categoria = new Categoria
            {
                Nome = createDto.Nome,
                Slug = createDto.Slug
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            var categoriaDto = new CategoriaDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Slug = categoria.Slug
            };

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoriaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, UpdateCategoriaDto updateDto)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            if (updateDto.Nome != null) categoria.Nome = updateDto.Nome;
            if (updateDto.Slug != null) categoria.Slug = updateDto.Slug;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}