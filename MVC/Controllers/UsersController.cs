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
using System.Security.Cryptography;
using System.Text;

namespace MVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private MVCContext db = new MVCContext();

        // GET: Users
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            var utilizador = new User();
            utilizador.perfis = new[]
            {
                new SelectListItem{Value="0",Text="Administrador" },
                new SelectListItem{Value="1",Text="Funcionário" }
            };
            return View(utilizador);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create([Bind(Include = "UserID,nome,password,perfil,estado")] User user)
        {
            user.perfis = new[]
            {
                new SelectListItem{Value="0",Text="Administrador" },
                new SelectListItem{Value="1",Text="Funcionário" }
            };
            if (ModelState.IsValid)
            {
                //testar se já existe um user com o mesmo nome
                var t = db.Users.Where(u => u.nome.Equals(user.nome)).ToList();
                if(t!=null && t.Count > 0)
                {
                    ModelState.AddModelError("nome", "Já existe um utilizador com esse nome.");
                    return View(user);
                }
                //validar password
                if(user.password==null || user.password.Trim().Length < 5)
                {
                    ModelState.AddModelError("password", "Password não é válida");
                    return View(user);
                }
                //encriptar password
                HMACSHA512 hMACSHA512 = new HMACSHA512(new byte[] { 1 });
                var password=hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(user.password));
                user.password=Convert.ToBase64String(password);
                user.estado = true;
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //testar se é admin
            if (User.IsInRole("Funcionário"))
            {
                //verificar se o id é igual ao id do user autenticado
                var t = db.Users.Where(u => u.nome == User.Identity.Name && id == u.UserID).FirstOrDefault();
                if (t == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                user.perfis = new[]
                {
                    new SelectListItem{Value="1",Text="Funcionário" }
                };
            }
            else
            {
                user.perfis = new[]
                {
                    new SelectListItem{Value="0",Text="Administrador" },
                    new SelectListItem{Value="1",Text="Funcionário" }
                };
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserID,nome,perfil,estado")] User user)
        {
            if (ModelState.IsValid)
            {
                var u = await db.Users.FindAsync(user.UserID);
                u.nome=user.nome;
                u.perfil=user.perfil;
                u.estado=user.estado;

                db.Entry(u).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //testar se é admin
            if (User.IsInRole("Funcionário"))
            {
                //verificar se o id é igual ao id do user autenticado
                var t = db.Users.Where(u => u.nome == User.Identity.Name && user.UserID == u.UserID).FirstOrDefault();
                if (t == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                user.perfis = new[]
                {
                    new SelectListItem{Value="1",Text="Funcionário" }
                };
            }
            else
            {
                user.perfis = new[]
                    {
                        new SelectListItem{Value="0",Text="Administrador" },
                        new SelectListItem{Value="1",Text="Funcionário" }
                    };
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles ="Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
