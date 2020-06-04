using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modeln;
using Servicen;
using Servicen.ServiceCliente;

namespace WebApplication3.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {

            var clientes = ClientesServices.ListarClientes();
             return View(clientes);
        }
        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Cliente cliente, ApplicationUserRole user)
        {
            return View(user);
        }
    }

}