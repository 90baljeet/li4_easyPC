using easyPC.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace easyPC.Models
{
    public class EncomendaComponente
    {
        [Key]
        public int Id { get; set; }

        // FK Encomenda
        public int EncomendaId { get; set; }
        [ForeignKey("EncomendaId")]
        public Encomenda Encomenda { get; set; }

        // FK Componente
        public int ComponenteId { get; set; }
        [ForeignKey("ComponenteId")]
        public Componente Componente { get; set; }
    }
}
