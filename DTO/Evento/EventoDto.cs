using System;

namespace Eventify.DTO.Evento
{
    public class EventoDto
    {
        public int EventoId { get; set; }
        public string Titolo { get; set; }
        public DateTime Data { get; set; }
        public string Luogo { get; set; }
        public string Descrizione { get; set; }
        public int Capienza { get; set; }
        public int ArtistaId { get; set; }
        public int BigliettiDisponibili { get; set; }
        public decimal PrezzoBiglietto { get; set; }
    }
}
