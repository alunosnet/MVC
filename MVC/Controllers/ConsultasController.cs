using MVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class ConsultasController : Controller
    {
        private MVCContext db = new MVCContext();

        // GET: Consultas
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult PesquisaCliente()
        {
            string nome = Request.Form["nome"];
            var clientes = db.Clientes.Where(c => c.Nome.Contains(nome));

            return View("PesquisaCliente", clientes.ToList());
        }
        public ActionResult PesquisaDinamica()
        {
            return View();
        }
        public JsonResult PesquisaNome(string nome)
        {
            var clientes = db.Clientes.Where(c => c.Nome.Contains(nome)).ToList();
            var lista = new List<Campos>();
            foreach (var c in clientes)
                lista.Add(new Campos() { nome = c.Nome });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MelhorCliente()
        {
            using(var context=new MVCContext())
            {
                string sql = @"SELECT nome,sum(valor_pago) as valor
                                FROM Estadias INNER JOIN Clientes
                                ON Estadias.clienteid=Clientes.Clienteid
                                GROUP BY Estadias.clienteid,nome
                                ORDER BY valor DESC";
                var melhor = context.Database.SqlQuery<Campos>(sql);
                if (melhor != null && melhor.ToList().Count > 0)
                    ViewBag.melhor = melhor.ToList()[0];
                else
                    ViewBag.melhor = null;
            }
            return View();
        }
        public class Campos
        {
            public string nome { get; set; }
            public decimal valor { get; set; }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}