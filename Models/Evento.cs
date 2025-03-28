using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventify.Models
{
    public class Evento
    {
        [Key]
        public int EventoId { get; set; }

        [Required(ErrorMessage = "Il titolo dell'evento è obbligatorio")]
        [StringLength(200, ErrorMessage = "Il titolo non può superare i 200 caratteri")]
        public string Titolo { get; set; }

        [Required(ErrorMessage = "La data dell'evento è obbligatoria")]
        [DataType(DataType.DateTime)]
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

        [ForeignKey("ArtistaId")]
        public virtual Artista Artista { get; set; }

        public virtual ICollection<Biglietto> Biglietti { get; set; }

        public int BigliettiVenduti { get; set; }

        [NotMapped]
        public int BigliettiDisponibili => Capienza - BigliettiVenduti;

        [Required(ErrorMessage = "Il prezzo del biglietto è obbligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un numero positivo")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrezzoBiglietto { get; set; }

        public Evento()
        {
            Biglietti = new HashSet<Biglietto>();
        }

        public bool VendiBiglietto()
        {
            if (BigliettiDisponibili > 0)
            {
                BigliettiVenduti++;
                return true;
            }
            return false;
        }

        public void AnnullaBiglietto()
        {
            if (BigliettiVenduti > 0)
            {
                BigliettiVenduti--;
            }
        }
    }
}