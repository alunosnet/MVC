using MVC.Data;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private MVCContext db = new MVCContext();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if(user.nome!=null && user.password != null)
            {
                //encriptar password
                HMACSHA512 hMACSHA512 = new HMACSHA512(new byte[] { 1 });
                var password = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(user.password));
                user.password = Convert.ToBase64String(password);
                foreach(var utilizador in db.Users.ToList())
                {
                    if(utilizador.nome==user.nome && utilizador.password == user.password)
                    {
                        //iniciar sessão
                        FormsAuthentication.SetAuthCookie(user.nome, false);
                        //redirecionar o user para ação
                        if (Request.QueryString["ReturnUrl"] == null)
                            return RedirectToAction("Index", "Home");
                        else
                            return Redirect(Request.QueryString["ReturnUrl"].ToString());
                    }
                }
            }
            ModelState.AddModelError("", "Login falhou. Tente novamente.");
            return View(user);
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