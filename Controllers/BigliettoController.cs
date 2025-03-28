using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventify.Data;
using Eventify.Models;
using Eventify.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Eventify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BigliettoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BigliettoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Amministratore")]

        public async Task<ActionResult<IEnumerable<BigliettoDto>>> GetBiglietti()
        {
            return await _context.Biglietti
                .Include(b => b.Evento)
                .Include(b => b.User)
                .Select(b => new BigliettoDto
                {
                    BigliettoId = b.BigliettoId,
                    EventoId = b.EventoId,
                    TitoloEvento = b.Evento.Titolo,
                    UserId = b.UserId,
                    UserName = b.User.UserName,
                    DataAcquisto = b.DataAcquisto,
                    Prezzo = b.Prezzo,
                    Stato = b.Stato
                })
                .ToListAsync();
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BigliettoDto>>> GetUserBiglietti()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            return await _context.Biglietti
                .Where(b => b.UserId == userId)
                .Include(b => b.Evento)
                .Select(b => new BigliettoDto
                {
                    BigliettoId = b.BigliettoId,
                    EventoId = b.EventoId,
                    TitoloEvento = b.Evento.Titolo,
                    UserId = b.UserId,
                    DataAcquisto = b.DataAcquisto,
                    Prezzo = b.Prezzo,
                    Stato = b.Stato
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BigliettoDto>> GetBiglietto(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var biglietto = await _context.Biglietti
                .Include(b => b.Evento)
                .FirstOrDefaultAsync(b => b.BigliettoId == id && b.UserId == userId);

            if (biglietto == null)
            {
                return NotFound();
            }

            return new BigliettoDto
            {
                BigliettoId = biglietto.BigliettoId,
                EventoId = biglietto.EventoId,
                TitoloEvento = biglietto.Evento.Titolo,
                UserId = biglietto.UserId,
                DataAcquisto = biglietto.DataAcquisto,
                Prezzo = biglietto.Prezzo,
                Stato = biglietto.Stato
            };
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BigliettoDto>> PostBiglietto(AcquistoBigliettoDto acquistoDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var evento = await _context.Eventi.FindAsync(acquistoDto.EventoId);
            if (evento == null)
            {
                return NotFound("Evento non trovato");
            }

            if (evento.BigliettiDisponibili < acquistoDto.Quantita)
            {
                return BadRequest("Non ci sono abbastanza biglietti disponibili");
            }

            var biglietti = new List<Biglietto>();
            for (int i = 0; i < acquistoDto.Quantita; i++)
            {
                var biglietto = new Biglietto
                {
                    EventoId = acquistoDto.EventoId,
                    UserId = userId,
                    DataAcquisto = DateTime.UtcNow,
                    Prezzo = evento.PrezzoBiglietto,
                    Stato = "Acquistato"
                };
                biglietti.Add(biglietto);
            }

            evento.BigliettiVenduti += acquistoDto.Quantita;

            _context.Biglietti.AddRange(biglietti);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);

            var bigliettoDto = new BigliettoDto
            {
                BigliettoId = biglietti[0].BigliettoId,
                EventoId = biglietti[0].EventoId,
                TitoloEvento = evento.Titolo,
                UserId = biglietti[0].UserId,
                UserName = user?.UserName,
                DataAcquisto = biglietti[0].DataAcquisto,
                Prezzo = biglietti[0].Prezzo,
                Stato = biglietti[0].Stato,
                QuantitaAcquistata = acquistoDto.Quantita
            };

            return CreatedAtAction("GetBiglietto", new { id = biglietti[0].BigliettoId }, bigliettoDto);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBiglietto(int id)
        {
            var biglietto = await _context.Biglietti
                .Include(b => b.User)
                .Include(b => b.Evento)
                .FirstOrDefaultAsync(b => b.BigliettoId == id);

            if (biglietto == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (biglietto.UserId != userId)
            {
                return Unauthorized();
            }

            if (biglietto.Evento.Data <= DateTime.UtcNow)
            {
                return BadRequest("Non è possibile cancellare un biglietto per un evento già passato.");
            }

            biglietto.Evento.BigliettiVenduti--;

            _context.Biglietti.Remove(biglietto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
