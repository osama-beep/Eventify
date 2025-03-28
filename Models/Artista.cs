using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eventify.Models
{
    public class Artista
    {
        [Key]
        public int ArtistaId { get; set; }

        [Required(ErrorMessage = "Il nome dell'artista è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Nome { get; set; }

        [StringLength(50, ErrorMessage = "Il genere non può superare i 50 caratteri")]
        public string Genere { get; set; }

        [StringLength(1000, ErrorMessage = "La biografia non può superare i 1000 caratteri")]
        public string Biografia { get; set; }

        public virtual ICollection<Evento> Eventi { get; set; }

        public Artista()
        {
            Eventi = new HashSet<Evento>();
        }
    }
}