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
    public class QuartosController : Controller
    {
        private MVCContext db = new MVCContext();

        // GET: Quartos
        public async Task<ActionResult> Index()
        {
            return View(await db.Quartos.ToListAsync());
        }

        // GET: Quartos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quarto quarto = await db.Quartos.FindAsync(id);
            if (quarto == null)
            {
                return HttpNotFound();
            }
            return View(quarto);
        }

        // GET: Quartos/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quartos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create([Bind(Include = "ID,piso,lotacao,custo_dia,casa_banho")] Quarto quarto)
        {
            if (ModelState.IsValid)
            {
                db.Quartos.Add(quarto);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(quarto);
        }

        // GET: Quartos/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quarto quarto = await db.Quartos.FindAsync(id);
            if (quarto == null)
            {
                return HttpNotFound();
            }
            return View(quarto);
        }

        // POST: Quartos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit([Bind(Include = "ID,piso,lotacao,custo_dia,casa_banho,estado")] Quarto quarto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quarto).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(quarto);
        }

        // GET: Quartos/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quarto quarto = await db.Quartos.FindAsync(id);
            if (quarto == null)
            {
                return HttpNotFound();
            }
            return View(quarto);
        }

        // POST: Quartos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Quarto quarto = await db.Quartos.FindAsync(id);
            db.Quartos.Remove(quarto);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
