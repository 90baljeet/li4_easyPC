using easyPC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace easyPC.Models
{
    public class Encomenda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Aguardando montagem";

        // Chave estrangeira
        public int ClienteId { get; set; }

        // Navegação
        [ForeignKey("ClienteId")]

        [ValidateNever]
        public Cliente Cliente { get; set; }

        public ICollection<EncomendaComponente> EncomendaComponentes { get; set; } = new List<EncomendaComponente>();

        public int? MontagemId { get; set; }

        [ForeignKey("MontagemId")]

        [ValidateNever]
        public Montagem Montagem { get; set; }

    }
}
