using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventify.Models
{
    public class Biglietto
    {
        [Key]
        public int BigliettoId { get; set; }

        [Required]
        public int EventoId { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento Evento { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataAcquisto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prezzo { get; set; }

        [Required]
        [StringLength(20)]
        public string Stato { get; set; }

        public Biglietto()
        {
            DataAcquisto = DateTime.Now;
            Stato = "Valido";
        }
    }
}