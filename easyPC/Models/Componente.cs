using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace easyPC.Models
{
    public enum TipoComponente
    {
        CPU,
        RAM,
        Disco,
        Fonte,
        Caixa
    }

    public class Componente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TipoComponente Tipo { get; set; }

        [Required]
        [StringLength(50)]
        public string Marca { get; set; }

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        // Navegação
        public ICollection<EncomendaComponente> EncomendaComponentes { get; set; } = new List<EncomendaComponente>();
    }
}
