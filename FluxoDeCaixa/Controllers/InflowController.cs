using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluxoDeCaixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluxoDeCaixa.Repositories;
using FluxoDeCaixa.Models.ViewModels;

namespace FluxoDeCaixa.Controllers
{
    public class InflowController : Controller
    {

        private readonly InflowRepository inflowRepository;
        private readonly PersonRepository personRepository;
        public InflowController(NHibernate.ISession session)
        {
            inflowRepository = new InflowRepository(session);
            personRepository = new PersonRepository(session);
        }

        // GET: PersonController
        public ActionResult Index()
        {
            return View(inflowRepository.FindAll().ToList());
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
