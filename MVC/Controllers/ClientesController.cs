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
    public class ClientesController : Controller
    {
        private MVCContext db = new MVCContext();

        // GET: Clientes
        public async Task<ActionResult> Index()
        {
            return View(await db.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = await db.Clientes.FindAsync(id);
            //já inclui as estadias devido à lista que está no model dos clientes

            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ClienteID,Nome,Morada,CP,Email,Telefone,DataNascimento")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                //evitar email repetido
                var contar = db.Clientes.Where(c => c.Email == cliente.Email).Count();
                if (contar > 0)
                {
                    ModelState.AddModelError("Email", "O email já existe.");
                    return View(cliente);
                }
                db.Clientes.Add(cliente);
                await db.SaveChangesAsync();
                //guardar fotografia
                HttpPostedFileBase fotografia = Request.Files["fotografia"];
                if(fotografia != null && fotografia.ContentLength > 0)
                {
                    string imagem = Server.MapPath("~/Fotos/") + cliente.ClienteID + ".jpg";
                    fotografia.SaveAs(imagem);
                }
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = await db.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClienteID,Nome,Morada,CP,Email,Telefone,DataNascimento")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                //evitar email repetido
                var contar = db.Clientes.Where(c => c.Email == cliente.Email && c.ClienteID!=cliente.ClienteID).Count();
                if (contar > 0)
                {
                    ModelState.AddModelError("Email", "O email já existe.");
                    return View(cliente);
                }
                db.Entry(cliente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        [Authorize(Roles ="Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = await db.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cliente cliente = await db.Clientes.FindAsync(id);
            db.Clientes.Remove(cliente);
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
