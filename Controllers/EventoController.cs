using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Eventify.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Eventify.Data;
using System.Linq;
using Eventify.DTO.Evento;  // Modifica questa riga per importare i DTO dalla cartella corretta

namespace Eventify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventoDto>>> GetEventi()
        {
            return await _context.Eventi
                .Include(e => e.Artista)
                .Select(e => new EventoDto
                {
                    EventoId = e.EventoId,
                    Titolo = e.Titolo,
                    Data = e.Data,
                    Luogo = e.Luogo,
                    Descrizione = e.Descrizione,
                    Capienza = e.Capienza,
                    ArtistaId = e.ArtistaId,
                    NomeArtista = e.Artista.Nome,
                    BigliettiDisponibili = e.BigliettiDisponibili,
                    PrezzoBiglietto = e.PrezzoBiglietto

                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventoDto>> GetEvento(int id)
        {
            var evento = await _context.Eventi
                .Include(e => e.Artista)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (evento == null)
            {
                return NotFound();
            }

            return new EventoDto
            {
                EventoId = evento.EventoId,
                Titolo = evento.Titolo,
                Data = evento.Data,
                Luogo = evento.Luogo,
                Descrizione = evento.Descrizione,
                Capienza = evento.Capienza,
                ArtistaId = evento.ArtistaId,
                NomeArtista = evento.Artista.Nome,
                BigliettiDisponibili = evento.BigliettiDisponibili,
                PrezzoBiglietto = evento.PrezzoBiglietto,





            };
        }

        [HttpPost]
        [Authorize(Roles = "Amministratore")]

        public async Task<ActionResult<EventoDto>> PostEvento(CreateEventoDto createEventoDto)
        {
            var evento = new Evento
            {
                Titolo = createEventoDto.Titolo,
                Data = createEventoDto.Data,
                Luogo = createEventoDto.Luogo,
                Descrizione = createEventoDto.Descrizione,
                Capienza = createEventoDto.Capienza,
                ArtistaId = createEventoDto.ArtistaId,
                PrezzoBiglietto = createEventoDto.PrezzoBiglietto
            };

            _context.Eventi.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvento", new { id = evento.EventoId }, new EventoDto
            {
                EventoId = evento.EventoId,
                Titolo = evento.Titolo,
                Data = evento.Data,
                Luogo = evento.Luogo,
                Descrizione = evento.Descrizione,
                Capienza = evento.Capienza,
                ArtistaId = evento.ArtistaId,
                NomeArtista = evento.Artista?.Nome,
                BigliettiDisponibili = evento.BigliettiDisponibili,
                PrezzoBiglietto = evento.PrezzoBiglietto
            });
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Amministratore")]

        public async Task<IActionResult> PutEvento(int id, CreateEventoDto updateEventoDto)
        {
            var evento = await _context.Eventi.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            evento.Titolo = updateEventoDto.Titolo;
            evento.Data = updateEventoDto.Data;
            evento.Luogo = updateEventoDto.Luogo;
            evento.Descrizione = updateEventoDto.Descrizione;
            evento.Capienza = updateEventoDto.Capienza;
            evento.ArtistaId = updateEventoDto.ArtistaId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Amministratore")]

        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventi.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _context.Eventi.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
