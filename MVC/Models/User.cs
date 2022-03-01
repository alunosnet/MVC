using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Tem de indicar o nome do utilizador")]
        [StringLength(50)]
        [MinLength(2, ErrorMessage = "Nome é muito pequeno")]
        public string nome { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Palavra passe")]
        public string password { get; set; }

        [Required(ErrorMessage = "Indique o perfil")]
        [Display(Name = "Perfil do utilizador")]
        public int perfil { get; set; }

        [Display(Name = "Estado da conta")]
        public bool estado { get; set; }

        //dropdown para perfil
        public IEnumerable<System.Web.Mvc.SelectListItem> perfis { get; set; }
    }
}