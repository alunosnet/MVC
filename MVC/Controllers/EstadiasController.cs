using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC.Data;
using MVC.Models;

namespace MVC.Controllers
{
    [Authorize]
    public class EstadiasController : Controller
    {
        private MVCContext db = new MVCContext();

        // GET: Estadias
        public async Task<ActionResult> Index()
        {
            var estadias = db.Estadias.Include(e => e.cliente).Include(e => e.quarto);
            return View(await estadias.ToListAsync());
        }

        // GET: Estadias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadia estadia = await db.Estadias.FindAsync(id);
            if (estadia == null)
            {
                return HttpNotFound();
            }
            //TODO: incluir cliente e quarto
            var cliente = await db.Clientes.FindAsync(estadia.ClienteID);
            var quarto = await db.Quartos.FindAsync(estadia.QuartoID);
            estadia.quarto = quarto;
            estadia.cliente = cliente;
            return View(estadia);
        }

        // GET: Estadias/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome");
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado==true), "ID", "ID");
            return View();
        }

        // POST: Estadias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EstadiaID,data_entrada,data_saida,valor_pago,ClienteID,QuartoID")] Estadia estadia)
        {
            if (ModelState.IsValid)
            {
                estadia.data_saida = estadia.data_entrada;
                estadia.valor_pago = 0;
                db.Estadias.Add(estadia);
                //alterar o estado do quarto
                var quarto = db.Quartos.Find(estadia.QuartoID);
                quarto.estado = false;
                db.Entry(quarto).CurrentValues.SetValues(quarto);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", estadia.ClienteID);
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado == true), "ID", "ID", estadia.QuartoID);
            return View(estadia);
        }

        // GET: Estadias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadia estadia = await db.Estadias.FindAsync(id);
            if (estadia == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", estadia.ClienteID);
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado == true || q.ID==estadia.QuartoID), "ID", "ID", estadia.QuartoID);
            return View(estadia);
        }

        // POST: Estadias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EstadiaID,data_entrada,data_saida,valor_pago,ClienteID,QuartoID")] Estadia estadia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", estadia.ClienteID);
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado == true || q.ID == estadia.QuartoID), "ID", "ID", estadia.QuartoID);
            return View(estadia);
        }

        //// GET: Estadias/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Estadia estadia = await db.Estadias.FindAsync(id);
        //    if (estadia == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(estadia);
        //}

        //// POST: Estadias/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Estadia estadia = await db.Estadias.FindAsync(id);
        //    db.Estadias.Remove(estadia);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        public async Task<ActionResult> ListarEstadiasEmCurso()
        {
            var estadias = db.Estadias.Where( e => e.valor_pago==0 && e.data_entrada==e.data_saida)
                                        .Include(e => e.cliente)
                                        .Include(e => e.quarto);
            return View(await estadias.ToListAsync());
        }
        public async Task<ActionResult> ProcessaSaida(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadia estadia = await db.Estadias.FindAsync(id);
            if(estadia== null)
            {
                return HttpNotFound();
            }
            //dados do quarto
            var dadosQuarto = await db.Quartos.FindAsync(estadia.QuartoID);
            //calcular o valor a pagar
            TimeSpan dias = DateTime.Now.Date.Subtract(estadia.data_entrada.Date);
            int diasPagar=(int)(dias.TotalDays<=0 ? 1 : dias.TotalDays);
            estadia.valor_pago = diasPagar * dadosQuarto.custo_dia;
            estadia.data_saida = DateTime.Now;
            ViewBag.dias = diasPagar;
            //dados cliente
            estadia.cliente = await db.Clientes.FindAsync(estadia.ClienteID);
            return View(estadia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcessaSaida(Estadia estadia)
        {
            //atualizar a estadia
            db.Entry(estadia).State = EntityState.Modified;
            //atualizar o quarto
            var quarto=await db.Quartos.FindAsync(estadia.QuartoID);
            quarto.estado = true;
            db.Entry(quarto).CurrentValues.SetValues(quarto);
            await db.SaveChangesAsync();

            //redirecionar
            return RedirectToAction("ListarEstadiasEmCurso");
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
