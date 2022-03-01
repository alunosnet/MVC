using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Estadia
    {
        [Key]
        public int EstadiaID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Entrada")]
        [Required(ErrorMessage = "Tem de indicar a data de entrada")]
        public DateTime data_entrada { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        [Display(Name = "Data de Saída")]
        public DateTime data_saida { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Valor pago")]
        public decimal valor_pago { get; set; }

        //cliente
        [ForeignKey("cliente")]
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Tem de indicar um cliente")]
        public int ClienteID { get; set; }
        public Cliente cliente { get; set; }
        //quarto
        [ForeignKey("quarto")]
        [Display(Name = "Quarto")]
        [Required(ErrorMessage = "Tem de indicar um quarto")]
        public int QuartoID { get; set; }
        public Quarto quarto { get; set; }
    }
}