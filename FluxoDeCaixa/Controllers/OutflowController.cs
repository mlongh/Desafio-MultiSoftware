using FluxoDeCaixa.Models;
using FluxoDeCaixa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FluxoDeCaixa.Models.ViewModels;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Controllers
{
    public class OutflowController : Controller
    {
        private readonly OutflowRepository outflowRepository;
        private readonly PersonRepository personRepository;
        public OutflowController(NHibernate.ISession session)
        {
            outflowRepository = new OutflowRepository(session);
            personRepository = new PersonRepository(session);
        }

        // GET: PersonController
        public ActionResult Index()
        {
            return View(outflowRepository.FindAll().ToList());
        }


        [HttpGet]
        public ActionResult SearchFilter(Filter filter)
        {
            var returnFilter = outflowRepository.SearchFilter(filter);
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
                ViewBag.msgMin = "Saldo abaixo do mínimo";
                ViewBag.text = "Saldo abaixo do mínimo cadastrado";
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
