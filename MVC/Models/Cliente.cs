using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "Tem de indicar o nome do cliente")]
        [StringLength(80)]
        [MinLength(3, ErrorMessage = "Nome muito pequeno. Deve ter pelo menos 3 letras")]
        [UIHint("Insira o nome, pelo menos 3 letras")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Tem de indicar a morada do cliente")]
        [StringLength(110)]
        [MinLength(3, ErrorMessage = "Morada muito pequena. Deve ter pelo menos 3 letras")]
        [UIHint("Insira a morada do cliente, pelo menos 3 letras")]
        public string Morada { get; set; }

        [Required(ErrorMessage = "Tem de indicar o código postal do cliente")]
        [StringLength(8)]
        [MinLength(8, ErrorMessage = "O código postal tem de ter 8 números")]
        [Display(Name = "Código Postal")]
        public string CP { get; set; }

        [Required(ErrorMessage = "Tem de indicar o email do cliente")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(15)]
        [MinLength(9, ErrorMessage = "Deve ter no mínimo 9 digitos")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Tem de a data de nascimento do cliente")]
        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [NotMapped]
        public int Idade { get; set; }
        //lista das estadias
        public virtual List<Estadia> listaEstadias { get; set; }
    }
}