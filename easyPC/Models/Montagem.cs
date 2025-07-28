using easyPC.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace easyPC.Models
{
    public class Montagem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EtapaAtual { get; set; } = 0; // 0 = não iniciada

        [DataType(DataType.DateTime)]
        public DateTime DataInicio { get; set; }

        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(Montagem), nameof(ValidarDataFim))]
        public DateTime DataFim { get; set; }

        public static ValidationResult? ValidarDataFim(DateTime dataFim, ValidationContext context)
        {
            var montagem = context.ObjectInstance as Montagem;
            if (montagem == null || dataFim >= montagem.DataInicio)
                return ValidationResult.Success;

            return new ValidationResult("A data de fim deve ser posterior à data de início.");
        }

        // FK para Encomenda (1:1)
        public int EncomendaId { get; set; }

        [ForeignKey("EncomendaId")]
        [ValidateNever]
        public Encomenda Encomenda { get; set; }

    }
}
