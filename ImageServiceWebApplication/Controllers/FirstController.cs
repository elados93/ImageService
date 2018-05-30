using ImageServiceWebApplication.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class FirstController : Controller
    {
        static List<Employee> students = new List<Employee>()
        {
          new Employee  { FirstName = "Elad", LastName = "Aharon",  ID = 311200786},
          new Employee  { FirstName = "Shahar", LastName = "Palmor", ID = 307929927},
        };
        static ConfigModel configModel = new ConfigModel();

        // GET: First
        public ActionResult Config()
        {
            return View(configModel);
        }

        [HttpGet]
        public ActionResult ImageWeb()
        {
            return View(students);
        }

        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }

        [HttpPost]
        public JObject GetEmployee(string name, int salary)
        {
            foreach (var student in students)
            {
                JObject data = new JObject();
                data["FirstName"] = student.FirstName;
                data["LastName"] = student.LastName;
                data["ID"] = student.ID;
                return data;
            }
            return null;
        }

        // GET: First/Details
        public ActionResult Details()
        {
            return View(students);
        }

        // GET: First/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: First/Create
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            try
            {
                students.Add(emp);

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        // GET: First/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (Employee emp in students)
            {
                if (emp.ID.Equals(id))
                {
                    return View(emp);
                }
            }
            return View("Error");
        }

        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee empT)
        {
            try
            {
                foreach (Employee emp in students)
                {
                    if (emp.ID.Equals(id))
                    {
                        emp.copy(empT);
                        return RedirectToAction("Config");
                    }
                }

                return RedirectToAction("Config");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Employee emp in students)
            {
                if (emp.ID.Equals(id))
                {
                    students.RemoveAt(i);
                    return RedirectToAction("Details");
                }
                i++;
            }
            return RedirectToAction("Error");
        }
    }
}
