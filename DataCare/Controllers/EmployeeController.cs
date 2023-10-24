using DataCare.Helpers;
using DataCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DataCare.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private DataStore DataStore { get; set; }
        private List<Employee> employees;

        private List<Contract> contracts = new List<Contract> {};
        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Employees()
        {
            DataStore = new DataStore(HttpContext.Session);
            employees = DataStore.GetEmployees();
            return View(employees);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            DataStore = new DataStore(HttpContext.Session);
            employee.EmployeeId = new Random().Next();
            DataStore.SaveEmployee(employee);

            return RedirectToAction("Employees");
        }

        public IActionResult Contracts()
        {
            DataStore = new DataStore(HttpContext.Session);
            contracts = DataStore.GetContracts();
            return View(contracts);
        }

        public IActionResult Contract()
        {
            DataStore = new DataStore(HttpContext.Session);
            employees = DataStore.GetEmployees();
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FirstName");

            return View();
        }

        [HttpPost]
        public IActionResult SaveContract(Contract contract)
        {
            DataStore = new DataStore(HttpContext.Session);
            DataStore.SaveContract(contract);
            return RedirectToAction("Contracts", "Employee");
        }

        public IActionResult Appraisals()
        {
            DataStore = new DataStore(HttpContext.Session);
            var appraisals = DataStore.GetAppraisals();
            return View(appraisals);
        }

        public IActionResult ConductAppraisal()
        {
            DataStore = new DataStore(HttpContext.Session);
            var objectives = DataStore.GetAppraisalObjectives();
            var appraisalPeriods = DataStore.GetAppraisalPeriods();
            var employees = DataStore.GetEmployees();

            ViewBag.Objectives = new SelectList(objectives, "ObjectiveId", "Name");
            ViewBag.AppraisalPeriods = new SelectList(appraisalPeriods, "AppraisalPeriodId", "Name");
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FirstName");

            return View();
        }

        [HttpPost]
        public IActionResult SaveAppraisal(Appraisal appraisal)
        {
            // An employee carrying out an appraisal must HAVE an active contract.
            if (!EmployeeHasActiveContract(appraisal.EmployeeId))
                ModelState.AddModelError("EmployeeId", "Employee must have an active contract to conduct an appraisal.");

            // An appraisal MUST be done within the Reporting dates of that appraisal period.
            if (!AppraisalPeriodValidity(appraisal.AppraisalPeriodId))
                ModelState.AddModelError("ReportingDate", "Appraisal must be conducted within the reporting dates of the appraisal period.");

            // The appraisal created MUST go through a workflow process where it is approved by the employee’s Supervisor
            if (appraisal.WorkflowStatus != "Approved")
                ModelState.AddModelError("ReportingDate", "Appraisal must be approved by the employees supervisor.");

            if (!ModelState.IsValid)
                return RedirectToAction("AppraisalError");

            DataStore.SaveAppraisal(appraisal);
            return RedirectToAction("Appraisals");
        }

        public IActionResult AppraisalError()
        {
            return View();
        }

        private bool EmployeeHasActiveContract(int employeeId)
        {
            DataStore = new DataStore(HttpContext.Session);
            var contracts = DataStore.GetContracts();

            var contract = contracts.Find(x => x.EmployeeId == employeeId);
            if (contract != null)
                return contract.Status == "Active";

            return false;
        }

        private bool AppraisalPeriodValidity(int appraisalPeriodId)
        {
            var appraisalPeriods = DataStore.GetAppraisalPeriods();
            var appraisalPeriod = appraisalPeriods.Find(ap => ap.AppraisalPeriodId == appraisalPeriodId);
            DateTime appraisalReportDate = DateTime.Now;
            
            return (appraisalPeriod == null ||
                   (appraisalReportDate > appraisalPeriod.StartDate &&
                   appraisalReportDate < appraisalPeriod.EndDate));
        }
    }
}
