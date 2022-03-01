using MVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Helper
{
    public static class Utils
    {
        public static string UserId(this HtmlHelper htmlHelper,System.Security.Principal.IPrincipal utilizador)
        {
            string iduser = "";

            using(var context=new MVCContext())
            {
                var consulta = context.Database.SqlQuery<int>("SELECT UserID FROM Users WHERE nome=@p0", utilizador.Identity.Name);
                if (consulta.ToList().Count > 0)
                    iduser = String.Format("{0}", consulta.ToList()[0]);
            }

            return iduser;
        }
    }
}