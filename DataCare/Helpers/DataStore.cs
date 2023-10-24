using DataCare.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace DataCare.Helpers
{
    public class DataStore
    {
        private ISession session;

        public DataStore(ISession session)
        {
            this.session = session;
        }

        public List<Employee> GetEmployees()
        {
            byte[] byteArray = session.Get("Employees");

            if (byteArray != null)
            {
                string json = Encoding.UTF8.GetString(byteArray);

                var employees = JsonConvert.DeserializeObject<List<Employee>>(json);

                return employees;
            }
            else
            {
                // return a demo list
                var _employees = new List<Employee>
                {
                    new Employee { EmployeeId = new Random().Next(), FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = new DateTime(1990, 1, 15), NIN = "12345", PhoneNumber = "123-456-7890" },
                    new Employee { EmployeeId = new Random().Next(), FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", DateOfBirth = new DateTime(1985, 5, 20), NIN = "67890", PhoneNumber = "987-654-3210" }
                };

                SaveEmployee(null, _employees);

                return _employees;
            }
        }

        public void SaveEmployee(Employee employee = null, List<Employee> employees = null)
        {
            if (employees == null)
            {
                employees = GetEmployees();
                employees.Add(employee);
            }

            string json = JsonConvert.SerializeObject(employees);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            session.Remove("Employees");
            session.Set("Employees", byteArray);
        }

        public List<AppraisalPeriod> GetAppraisalPeriods()
        {
            var _appraisalPeriods = new List<AppraisalPeriod>();
            byte[] byteArray = session.Get("AppraisalPeriods");

            if (byteArray != null)
            {
                string json = Encoding.UTF8.GetString(byteArray);

                _appraisalPeriods = JsonConvert.DeserializeObject<List<AppraisalPeriod>>(json);
            }
            else
            {
                _appraisalPeriods = new List<AppraisalPeriod> {
                    new AppraisalPeriod { AppraisalPeriodId = new Random().Next(), Name = "Quarter 1", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 12, 30) },
                    new AppraisalPeriod { AppraisalPeriodId = new Random().Next(), Name = "Quarter 2", StartDate = new DateTime(2023, 4, 1), EndDate = new DateTime(2023, 6, 30) },
                    new AppraisalPeriod { AppraisalPeriodId = new Random().Next(), Name = "Quarter 3", StartDate = new DateTime(2023, 7, 1), EndDate = new DateTime(2023, 9, 30) }
                };

                SaveAppraisalPeriods(null, _appraisalPeriods);
            }

            return _appraisalPeriods;
        }

        public void SaveAppraisalPeriods(AppraisalPeriod appraisalPeriod = null, List<AppraisalPeriod> appraisalPeriods = null)
        {
            if (appraisalPeriods == null)
            {
                appraisalPeriods = GetAppraisalPeriods();
                appraisalPeriods.Add(appraisalPeriod);
            }

            string json = JsonConvert.SerializeObject(appraisalPeriods);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            session.Remove("AppraisalPeriods");
            session.Set("AppraisalPeriods", byteArray);
        }

        public List<AppraisalObjective> GetAppraisalObjectives()
        {
            var _employees = GetEmployees();
            var _objectives = new List<AppraisalObjective>();
            byte[] byteArray = session.Get("AppraisalObjectives");

            if (byteArray != null)
            {
                string json = Encoding.UTF8.GetString(byteArray);

                _objectives = JsonConvert.DeserializeObject<List<AppraisalObjective>>(json);
            }
            else
            {
                _objectives.Add(
                        new AppraisalObjective { ObjectiveId = new Random().Next(), Employee = _employees.FirstOrDefault(), 
                                                 ActivityStartDate = new DateTime(2023, 1, 1), 
                                                 ActivityEndDate = new DateTime(2023, 12, 30), Name = "Objective 11" }
    
                    );

                SaveAppraisalObjectives(null, _objectives);
            }

            return _objectives;
        }

        public void SaveAppraisalObjectives(AppraisalObjective appraisalObjective = null, 
                                            List<AppraisalObjective> appraisalObjectives = null)
        {
            if (appraisalObjectives == null)
            {
                appraisalObjectives = GetAppraisalObjectives();
                appraisalObjectives.Add(appraisalObjective);
            }

            string json = JsonConvert.SerializeObject(appraisalObjectives);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            session.Remove("AppraisalObjectives");
            session.Set("AppraisalObjectives", byteArray);
        }

        internal List<Appraisal> GetAppraisals()
        {
            var _appraisals = new List<Appraisal>();
            byte[] byteArray = session.Get("Appraisals");

            if (byteArray != null)
            {
                string json = Encoding.UTF8.GetString(byteArray);

                _appraisals = JsonConvert.DeserializeObject<List<Appraisal>>(json);
            }
            else
            {
                _appraisals.Add(new Appraisal { Comment = "asd", ApproverComment = "poi", Rating = 4, WorkflowStatus = "Rejected" });
            }

            return _appraisals;
        }

        public void SaveAppraisal(Appraisal appraisal)
        {
            var appraisals = GetAppraisals();
            appraisals.Add(appraisal);

            string json = JsonConvert.SerializeObject(appraisals);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            session.Remove("Appraisals");
            session.Set("Appraisals", byteArray);
        }

        internal List<Contract> GetContracts()
        {
            var _employees = GetEmployees();
            byte[] byteArray = session.Get("Contracts");

            if (byteArray != null)
            {
                string json = Encoding.UTF8.GetString(byteArray);

                var contracts = JsonConvert.DeserializeObject<List<Contract>>(json);
                contracts.ForEach(c =>
                {
                    c.Employee = _employees.Find(e => e.EmployeeId == c.EmployeeId);
                });

                return contracts;
            }
            else
            {
                var _employee = _employees?.FirstOrDefault();
                var _contracts = new List<Contract>
                {
                    new Contract
                    {
                        EmployeeId = _employee.EmployeeId,
                        Employee = _employee,
                        Department = "IT",
                        EndDate = new DateTime(2025, 6, 30),
                        Position = "Manager",
                        StartDate = new DateTime(2023, 6, 1),
                        Status = "Active"

                    }
                };

                SaveContract(null, _contracts);

                return _contracts;
            }
        }

        public void SaveContract(Contract contract = null, List<Contract> contracts = null)
        {
            if (contracts == null)
            {
                contracts = GetContracts();
                contracts.Add(contract);
            }

            string json = JsonConvert.SerializeObject(contracts);

            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            session.Remove("Contracts");
            session.Set("Contracts", byteArray);
        }
    }
}
