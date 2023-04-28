using FluxoDeCaixa.Models;
using FluxoDeCaixa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PersonRepository personRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _contxt;
        public HomeController(ILogger<HomeController> logger, NHibernate.ISession session, Microsoft.AspNetCore.Http.IHttpContextAccessor contxt)
        {
            personRepository = new PersonRepository(session);
            _logger = logger;
            _contxt = contxt;
        }

        [HttpPost]
        public IActionResult Logar(string username, string password)
        {
            
            var user = personRepository.FindByUsername(username);
            if(username == null && password == null)
            {
                ViewBag.Message = "Usuário ou senha incorretos";
                return View("Login");
            }
            else
            {
                if((user?.Password ?? string.Empty) != password )
                {
                    ViewBag.Message = "Usuario ou senha incorretos";
                    return View("Login");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(user);
                    _contxt.HttpContext.Session.SetString("User", jsonString);

                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult Login()
        {
          
            return View();
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
