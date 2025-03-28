using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Eventify.Models;
using Eventify.DTO;

namespace Eventify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Amministratore")]
    public class RuoliController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RuoliController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("assegna")]
        public async Task<IActionResult> AssegnaRuolo(AssegnaRuoloDto assegnaRuoloDto)
        {
            var user = await _userManager.FindByIdAsync(assegnaRuoloDto.UserId);
            if (user == null)
            {
                return NotFound("Utente non trovato");
            }

            if (!await _roleManager.RoleExistsAsync(assegnaRuoloDto.Ruolo))
            {
                return BadRequest("Ruolo non valido");
            }

            var result = await _userManager.AddToRoleAsync(user, assegnaRuoloDto.Ruolo);
            if (result.Succeeded)
            {
                return Ok($"Ruolo {assegnaRuoloDto.Ruolo} assegnato con successo all'utente {user.UserName}");
            }

            return BadRequest("Errore nell'assegnazione del ruolo");
        }

        [HttpPost("rimuovi")]
        public async Task<IActionResult> RimuoviRuolo(AssegnaRuoloDto rimuoviRuoloDto)
        {
            var user = await _userManager.FindByIdAsync(rimuoviRuoloDto.UserId);
            if (user == null)
            {
                return NotFound("Utente non trovato");
            }

            if (!await _roleManager.RoleExistsAsync(rimuoviRuoloDto.Ruolo))
            {
                return BadRequest("Ruolo non valido");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, rimuoviRuoloDto.Ruolo);
            if (result.Succeeded)
            {
                return Ok($"Ruolo {rimuoviRuoloDto.Ruolo} rimosso con successo dall'utente {user.UserName}");
            }

            return BadRequest("Errore nella rimozione del ruolo");
        }

        [HttpGet("utenti")]
        public async Task<IActionResult> GetUtentiConRuoli()
        {
            var users = _userManager.Users;
            var utentiConRuoli = new List<UtenteConRuoliDto>();

            foreach (var user in users)
            {
                var ruoli = await _userManager.GetRolesAsync(user);
                utentiConRuoli.Add(new UtenteConRuoliDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Ruoli = ruoli.ToList()
                });
            }

            return Ok(utentiConRuoli);
        }
    }
}