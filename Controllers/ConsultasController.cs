using ProjectCRUD.Models.Clases;
using ProjectCRUD.Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjectCRUD.Controllers
{
    public class ConsultasController : Controller
    {


        // GET: Consultas
        [HttpPost]
        public ActionResult Ingreso(Usuario usuario)
        {
            UsuarioDao Dao = new UsuarioDao();
            String Usuario = usuario.usuarioEntrar;
            String Password = usuario.password;
            if (Dao.Login(usuario))
            {
                return View();

            }
            else
            {
                return Json(new { msg = "Error" });
            }

        }

        [HttpPost]
        public ActionResult Authorise(Usuario usuario)
        {
            UsuarioDao Dao = new UsuarioDao();

            if (Dao.Login(usuario))
            {
                Session["Usuario"] = usuario.usuarioEntrar;
                Session["Password"] = usuario.password;
                ViewBag.usuario = usuario.usuarioEntrar;
                return View("~/Views/Consultas/Principal.cshtml");
            }
            else
            {
                return View("Login", usuario);
            }

        }


        public ActionResult Principal()
        {
            return View();
        }

        public ActionResult MostrarUsuarios()
        {
            UsuarioDao Dao = new UsuarioDao();
            ModelState.Clear();
            return PartialView(Dao.MostrarUsuarios());
        }


        [HttpPost]
        public ActionResult InsertUsuario(Usuario usuario)
        {
            UsuarioDao Dao = new UsuarioDao();
            if (Dao.InsertUsuario(usuario))
            {
                return Json(new
                {
                    msg = "Successfully added "
                });
            }
            else
            {
                return Json(new
                {
                    msg = "Error added "
                });
            }
        }

        public ActionResult Mostrar(int Id)
        {
            UsuarioDao Dao = new UsuarioDao();
            ModelState.Clear();
            return PartialView(Dao.MostrarUsuario(Id));
        }

        [HttpPost]
        public ActionResult Update(Usuario usuario)
        {
            UsuarioDao Dao = new UsuarioDao();

            if (Dao.UpdateUsuario(usuario))
            {
                return Json(new { msg = "Success" });
            }
            else
            {
                return Json(new { msg = "Error" });
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            UsuarioDao Dao = new UsuarioDao();
            if (Dao.DeleteUsuario(Id))
            {
                return Json(new { msg = "Success" });
            }
            else
            {
                return Json(new { msg = "Error" });
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult InsertUsuarioLogin(Usuario usuario)
        {
            UsuarioDao Dao = new UsuarioDao();
            if (Dao.InsertUser(usuario))
            {
                return Json(new
                {
                    msg = "Successfully added "
                });
            }
            else
            {
                return Json(new
                {
                    msg = "Error added "
                });
            }
        }
    }
}