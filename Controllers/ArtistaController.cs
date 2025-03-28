using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eventify.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Eventify.Data;
using Eventify.DTO;

[ApiController]
[Route("api/[controller]")]
public class ArtistiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ArtistiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistaDto>>> GetArtisti()
    {
        return await _context.Artisti
            .Select(a => new ArtistaDto
            {
                ArtistaId = a.ArtistaId,
                Nome = a.Nome,
                Genere = a.Genere,
                Biografia = a.Biografia
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArtistaDto>> GetArtista(int id)
    {
        var artista = await _context.Artisti
            .Select(a => new ArtistaDto
            {
                ArtistaId = a.ArtistaId,
                Nome = a.Nome,
                Genere = a.Genere,
                Biografia = a.Biografia
            })
            .FirstOrDefaultAsync(a => a.ArtistaId == id);

        if (artista == null)
        {
            return NotFound();
        }
        return artista;
    }

    [HttpPost]
    public async Task<ActionResult<Artista>> PostArtista(CreateArtistaDto artistaDto)
    {
        var artista = new Artista
        {
            Nome = artistaDto.Nome,
            Genere = artistaDto.Genere,
            Biografia = artistaDto.Biografia
        };

        _context.Artisti.Add(artista);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetArtista), new { id = artista.ArtistaId }, artista);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutArtista(int id, UpdateArtistaDto artistaDto)
    {
        if (id != artistaDto.ArtistaId)
        {
            return BadRequest();
        }

        var artista = await _context.Artisti.FindAsync(id);
        if (artista == null)
        {
            return NotFound();
        }

        artista.Nome = artistaDto.Nome;
        artista.Genere = artistaDto.Genere;
        artista.Biografia = artistaDto.Biografia;

        _context.Entry(artista).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtista(int id)
    {
        var artista = await _context.Artisti.FindAsync(id);
        if (artista == null)
        {
            return NotFound();
        }

        _context.Artisti.Remove(artista);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}