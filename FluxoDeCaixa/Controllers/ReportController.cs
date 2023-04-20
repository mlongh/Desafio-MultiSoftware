using FluxoDeCaixa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluxoDeCaixa.Models.ViewModels;
using FluxoDeCaixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FluxoDeCaixa.Controllers
{
    public class ReportController : Controller
    {
        private readonly InflowRepository inflowRepository;
        private readonly OutflowRepository outflowRepository;
        public ReportController(NHibernate.ISession session)
        {
            inflowRepository = new InflowRepository(session);
            outflowRepository = new OutflowRepository(session);
        }

        // GET: ReportController
        public ActionResult Index()
        {
            ReportFormViewModel report = new ReportFormViewModel();

            report.Inflow = inflowRepository.FindAll();
            report.Outflow = outflowRepository.FindAll();

            return View(report);
        }


        [HttpGet]
        public ActionResult SearchFilter(Filter filter)
        {
            ReportFormViewModel returnFilter = new ReportFormViewModel();
            returnFilter.Outflow = outflowRepository.SearchFilter(filter);
            returnFilter.Inflow = inflowRepository.SearchFilter(filter);
            return View("Index", returnFilter);
        }

        // GET: ReportController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReportController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
