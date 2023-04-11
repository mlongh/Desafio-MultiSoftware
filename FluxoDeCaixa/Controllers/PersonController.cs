using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluxoDeCaixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluxoDeCaixa.Repositories;

namespace FluxoDeCaixa.Controllers
{
    public class PersonController : Controller
    {

        private readonly PersonRepository personRepository; 
        public PersonController(NHibernate.ISession session) => 
            personRepository = new PersonRepository(session);

        // GET: PersonController
        public ActionResult Index()
        {
            return View(personRepository.FindAll().ToList());
        }

        [HttpGet]
        public ActionResult SearchFilter(Person person)
        {
            var personReturn =  personRepository.FindByName(person.Name);
            return View("Index", personReturn);
        }


        // GET: PersonController/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if(id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Person person = await personRepository.FindByID(id.Value);
            if(person == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            } 

            return View(person);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind("Id, Name, Email, Salary, AccountLimit, MinimumValue")]
            Person person)
        {
           if(ModelState.IsValid)
            {
                await personRepository.Add(person);
                return RedirectToAction("Index");
            }
         
               return View(person);
        }

        // GET: PersonController/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Person person = await personRepository.FindByID(id.Value);
            if (person == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(person);
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind("Id, Name, Email, Salary, AccountLimit, MinimumValue")]
            Person person)
        {
            if (ModelState.IsValid)
            {
                await personRepository.Update(person);
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: PersonController/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Person person = await personRepository.FindByID(id.Value);
            if (person == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(person);
        }

        // POST: PersonController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await personRepository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
