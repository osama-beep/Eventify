using System;
using System.ComponentModel.DataAnnotations;

namespace Eventify.DTO.Evento
{
    public class CreateEventoDto
    {
        [Required(ErrorMessage = "Il titolo dell'evento è obbligatorio")]
        [StringLength(200, ErrorMessage = "Il titolo non può superare i 200 caratteri")]
        public string Titolo { get; set; }

        [Required(ErrorMessage = "La data dell'evento è obbligatoria")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Il luogo dell'evento è obbligatorio")]
        [StringLength(200, ErrorMessage = "Il luogo non può superare i 200 caratteri")]
        public string Luogo { get; set; }

        [Required(ErrorMessage = "La descrizione dell'evento è obbligatoria")]
        [StringLength(1000, ErrorMessage = "La descrizione non può superare i 1000 caratteri")]
        public string Descrizione { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La capienza deve essere un numero positivo")]
        public int Capienza { get; set; }

        [Required]
        public int ArtistaId { get; set; }

        [Required(ErrorMessage = "Il prezzo del biglietto è obbligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un numero positivo")]
        public decimal PrezzoBiglietto { get; set; }
    }
}