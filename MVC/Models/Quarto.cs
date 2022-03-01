using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Quarto
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Piso")]
        [Required(ErrorMessage = "Tem de indicar o piso do quarto")]
        [Range(1, 5, ErrorMessage = "O piso deve estar entre 1 e 5")]
        public int piso { get; set; }

        [Required(ErrorMessage = "Tem de indicar o número máximo de pessoas")]
        [Display(Name = "Lotação")]
        public int lotacao { get; set; }

        [Display(Name = "Custo diário")]
        [Required(ErrorMessage = "Tem de indicar o custo do quarto")]
        [DataType(DataType.Currency)]
        [Range(0, 1000, ErrorMessage = "O preço do quarto deve estar entre 0 e 1000")]
        public decimal custo_dia { get; set; }

        [Display(Name = "Casa de Banho")]
        public bool casa_banho { get; set; }
        [Display(Name = "Estado")]
        public bool estado { get; set; }

        public Quarto()
        {
            estado = true;
        }
    }
}