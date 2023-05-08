using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluxoDeCaixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluxoDeCaixa.Repositories;
using FluxoDeCaixa.Models.ViewModels;
using System.Globalization;
using System.Text.Json;

namespace FluxoDeCaixa.Controllers
{
    public class InflowController : Controller
    {

        private readonly InflowRepository inflowRepository;
        private readonly PersonRepository personRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _contxt;
        public InflowController(NHibernate.ISession session, Microsoft.AspNetCore.Http.IHttpContextAccessor contxt)
        {
            inflowRepository = new InflowRepository(session);
            personRepository = new PersonRepository(session);
            _contxt = contxt;
        }

        // GET: PersonController
        public ActionResult Index()
        {
            var pessoa = JsonSerializer.Deserialize<Person>(_contxt.HttpContext.Session.GetString("User"));

            if (pessoa.Id == 1)
            {
                ViewBag.Total = inflowRepository.SumAmount().ToString("C2", CultureInfo.CurrentCulture);
                ViewBag.Count = inflowRepository.CountAllInflows();
                return View(inflowRepository.FindAll().ToList());

            }
            else
            {
                ViewBag.TotalUser = inflowRepository.SumAmountUser(pessoa.Id).ToString("C2", CultureInfo.CurrentCulture);
                ViewBag.Count = inflowRepository.CountUserInflows(pessoa.Id);
                return View(inflowRepository.FindAllById(pessoa.Id).ToList());
            }
        }

        [HttpGet]
        public ActionResult SearchFilter(Filter filter)
        {
            var returnFilter = inflowRepository.SearchFilter(filter);
            double sum = 0;
            foreach(var inflow in returnFilter)
            {
                sum += inflow.InflowAmount;
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

            Inflow inflow = await inflowRepository.FindByID(id.Value);
            
            if (inflow == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(inflow);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {

            InflowFormViewModel inflowFormViewModel = new InflowFormViewModel() { };
            inflowFormViewModel.People = personRepository.FindAll().ToList();
            return View(inflowFormViewModel);
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Inflow Inflow)
        {
            Person person = await personRepository.FindByID(Inflow.Person.Id);
            person.Balance = person.Balance + Inflow.InflowAmount;
            Inflow.Person = person;

            await inflowRepository.Add(Inflow);
            await personRepository.Update(person);

            return RedirectToAction("Index");
        }

        // GET: PersonController/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Inflow inflow = await inflowRepository.FindByID(id.Value);
            if (inflow == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(inflow);
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            Inflow inflow)
        {
            if (ModelState.IsValid)
            {
                var antigo = await inflowRepository.FindByID(inflow.Id);
                antigo.InflowDate = inflow.InflowDate;
                antigo.InflowDescription = inflow.InflowDescription;
                
                await inflowRepository.Update(antigo);
                return RedirectToAction("Index");
            }

            return View(inflow);
        }

        // GET: PersonController/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Inflow inflow = await inflowRepository.FindByID(id.Value);
            Person person = await personRepository.FindByID(inflow.Person.Id);
            person.Balance = person.Balance - inflow.InflowAmount;
            inflow.Person = person;

            if (inflow == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            await personRepository.Update(person);
            return View(inflow);
        }

        // POST: PersonController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {

            await inflowRepository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
