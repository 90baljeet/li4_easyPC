
using System.ComponentModel.DataAnnotations;

namespace easyPC.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Telefone { get; set; }

        // Navegação
        public ICollection<Encomenda> Encomendas { get; set; } = new List<Encomenda>();
    }
}
