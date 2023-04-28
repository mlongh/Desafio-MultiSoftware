using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluxoDeCaixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluxoDeCaixa.Repositories;
using System.Text.Json;

namespace FluxoDeCaixa.Controllers
{
    public class PersonController : Controller
    {

        private readonly PersonRepository personRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _contxt;
        public PersonController(NHibernate.ISession session, Microsoft.AspNetCore.Http.IHttpContextAccessor contxt)
        {
            personRepository = new PersonRepository(session);
            _contxt = contxt;
        }

        // GET: PersonController
        public ActionResult Index()
        {
            ViewBag.Count = personRepository.CountPeople().ToString();
            return View(personRepository.FindAll().ToList());
        }

        [HttpGet]
        public ActionResult SearchFilter(Person person)
        {
            var personReturn = personRepository.FindByName(person.Name);
            return View("Index", personReturn);
        }



        // GET: PersonController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(long? id)
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

        // GET: PersonController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
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
            Person person)
        {
            if (ModelState.IsValid)
            {

                var antigo = await personRepository.FindByID(person.Id);
                antigo.Name = person.Name;
                antigo.Email = person.Email;
                antigo.Salary = person.Salary;
                antigo.AccountLimit = person.AccountLimit;
                antigo.MinimumValue = person.MinimumValue;
                antigo.Username = person.Username;

                await personRepository.Update(antigo);
                
                var pessoa = JsonSerializer.Deserialize<Person>(_contxt.HttpContext.Session.GetString("User"));
                if (pessoa.Id == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { person.Id });
                }
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
