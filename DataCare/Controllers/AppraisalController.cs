using DataCare.Helpers;
using DataCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;

namespace DataCare.Controllers
{
    public class AppraisalController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private List<AppraisalPeriod> appraisalPeriods = new List<AppraisalPeriod>();
        private List<AppraisalObjective> objectives = new List<AppraisalObjective>();
        private List<Employee> employees;
        private DataStore DataStore { get; set; } 
        public AppraisalController(ILogger<EmployeeController> logger) { 
            _logger = logger;
        }

        public IActionResult AppraisalPeriods()
        {
            DataStore = new DataStore(HttpContext.Session);
            appraisalPeriods = DataStore.GetAppraisalPeriods();

            return View(appraisalPeriods);
        }
        
        public IActionResult AppraisalPeriod()
        {
            return View();
        }

        public IActionResult Objectives()
        {
            DataStore = new DataStore(HttpContext.Session);
            objectives = DataStore.GetAppraisalObjectives();

            return View(objectives);
        }

        public IActionResult Objective()
        {
            DataStore = new DataStore(HttpContext.Session);
            var employees = DataStore.GetEmployees();
            ViewBag.Employees = new SelectList(employees, "FirstName", "LastName");

            return View();
        }

        [HttpPost]
        public IActionResult CreateObjective(AppraisalObjective appraisalObjective)
        {
            DataStore = new DataStore(HttpContext.Session);
            appraisalObjective.ObjectiveId = new Random().Next();
            DataStore.SaveAppraisalObjectives(appraisalObjective);

            return RedirectToAction("Objectives");
        }

        public IActionResult Create(AppraisalPeriod appraisal)
        {
            DataStore = new DataStore(HttpContext.Session);
            appraisal.AppraisalPeriodId = new Random().Next();
            DataStore.SaveAppraisalPeriods(appraisal);

            return RedirectToAction("AppraisalPeriods");
        }
    }
}
