using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventify.Models
{
    public class Biglietto
    {
        [Key]
        public int BigliettoId { get; set; }

        [Required(ErrorMessage = "L'ID dell'evento è obbligatorio.")]
        public int EventoId { get; set; }

        [ForeignKey("EventoId")]
        public virtual Evento Evento { get; set; }

        [Required(ErrorMessage = "L'ID dell'utente è obbligatorio.")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "La data di acquisto è obbligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "Inserire una data valida.")]
        public DateTime DataAcquisto { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero.")]
        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "Lo stato del biglietto è obbligatorio.")]
        [StringLength(20, ErrorMessage = "Lo stato non può superare i 20 caratteri.")]
        public string Stato { get; set; }

        public Biglietto()
        {
            DataAcquisto = DateTime.Now;
            Stato = "Valido";
        }
    }
}