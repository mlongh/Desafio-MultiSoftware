using FluxoDeCaixa.Models;
using FluxoDeCaixa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FluxoDeCaixa.Models.ViewModels;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.Json;
using FluentNHibernate.Testing.Values;

namespace FluxoDeCaixa.Controllers
{
    public class OutflowController : Controller
    {
        private readonly OutflowRepository outflowRepository;
        private readonly PersonRepository personRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _contxt;
        public OutflowController(NHibernate.ISession session, Microsoft.AspNetCore.Http.IHttpContextAccessor contxt)
        {
            outflowRepository = new OutflowRepository(session);
            personRepository = new PersonRepository(session);
            _contxt = contxt;
        }

        // GET: PersonController
        public ActionResult Index()
        {
            var pessoa = JsonSerializer.Deserialize<Person>(_contxt.HttpContext.Session.GetString("User"));

            if (pessoa.Id == 1)
            {
               
                ViewBag.Total = outflowRepository.SumAmount().ToString("C2", CultureInfo.CurrentCulture);
                ViewBag.Count = outflowRepository.CountAllOutflows();
                return View(outflowRepository.FindAll().ToList());

            }
            else
            {
                ViewBag.Total = outflowRepository.SumAmount().ToString("C2", CultureInfo.CurrentCulture);
                ViewBag.Count = outflowRepository.CountUserOutflows(pessoa.Id);
                return View(outflowRepository.FindAllById(pessoa.Id).ToList());
            }
            //return View(outflowRepository.FindAll().ToList());
        }


        [HttpGet]
        public ActionResult SearchFilter(Filter filter)
        {
            var returnFilter = outflowRepository.SearchFilter(filter);
            double sum = 0;
            foreach (var outflow in returnFilter)
            {
                sum += outflow.OutflowAmount;
            }
            ViewBag.Total = sum.ToString("C2", CultureInfo.CurrentCulture);

            return View("Index", returnFilter.ToList());
        }

        // GET: PersonController/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Outflow outflow = await outflowRepository.FindByID(id.Value);
            if (outflow == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(outflow);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            OutflowFormViewModel outflowFormViewModel = new OutflowFormViewModel() { };
            outflowFormViewModel.People = personRepository.FindAll().ToList();
            return View(outflowFormViewModel);
        }


        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Outflow outflow)
        {
            Person person = await personRepository.FindByID(outflow.Person.Id);
            person.Balance = person.Balance - outflow.OutflowAmount;
            outflow.Person = person;

            if (person.Balance < 0)
            {
                ViewBag.msg = "Saldo insuficiente para realizar operação";
                ViewBag.text = "Saldo abaixo de 0";
                OutflowFormViewModel outflowFormViewModel = new OutflowFormViewModel() { };
                outflowFormViewModel.People = personRepository.FindAll().ToList();
                return View("Create", outflowFormViewModel);
            }

            if (person.Balance < person.MinimumValue)
            {
                ViewBag.msgMin = "Cuidado! Saldo abaixo do mínimo seguro em conta. Mesmo assim a operacao será relizada.";
                ViewBag.text = "Mesmo assim a operacao será relizada.";
                OutflowFormViewModel outflowFormViewModel = new OutflowFormViewModel() { };
                outflowFormViewModel.People = personRepository.FindAll().ToList();
                await outflowRepository.Add(outflow);
                await personRepository.Update(person);
                return View("Create", outflowFormViewModel);
            }

            await outflowRepository.Add(outflow);
            await personRepository.Update(person);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Outflow outflow = await outflowRepository.FindByID(id.Value);
            if (outflow == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(outflow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
        [Bind("Id, OutDate, OutDescription, OutflowAmount")]
            Outflow outflow)
        {
            if (ModelState.IsValid)
            {
                await outflowRepository.Update(outflow);
                return RedirectToAction("Index");
            }

            return View(outflow);
        }

        // GET: PersonController/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Outflow outflow = await outflowRepository.FindByID(id.Value);
            Person person = await personRepository.FindByID(outflow.Person.Id);
            person.Balance = person.Balance + outflow.OutflowAmount;
            outflow.Person = person;

            if (outflow == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            await personRepository.Update(person);
            return View(outflow);
        }

        // POST: PersonController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await outflowRepository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
