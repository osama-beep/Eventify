using System;

namespace Eventify.DTO
{
    public class BigliettoDto
    {
        public int BigliettoId { get; set; }
        public int EventoId { get; set; }
        public string? TitoloEvento { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime DataAcquisto { get; set; }
        public decimal Prezzo { get; set; }
        public string? Stato { get; set; }
        public int QuantitaAcquistata { get; set; }

    }
}