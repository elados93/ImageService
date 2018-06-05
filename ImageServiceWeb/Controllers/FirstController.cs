using ImageServiceWeb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class FirstController : Controller
    {
        
        // GET: First
        public ActionResult Config()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ImageWeb()
        {
            return View();
        }

        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }

        //[HttpPost]
        //public JObject GetEmployee(string name, int salary)
        //{
        //    foreach (var empl in employees)
        //    {
              
        //            JObject data = new JObject();
        //            data["FirstName"] = empl.FirstName;
        //            data["LastName"] = empl.LastName;
        //            data["Salary"] = empl.ID;
        //            return data;
               
        //    }
        //    return null;
        //}

        // GET: First/Details
        public ActionResult Logs()
        {
            return View();
        }

        // GET: First/Create
        public ActionResult Photos()
        {
            return View();
        }

        //// POST: First/Create
        //[HttpPost]
        //public ActionResult Create(Employee emp)
        //{
        //    try
        //    {
        //        employees.Add(emp);

        //        return RedirectToAction("Details");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: First/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    foreach (Employee emp in employees)
        //    {
        //        if (emp.ID.Equals(id))
        //        {
        //            return View(emp);
        //        }
        //    }
        //    return View("Error");
        //}

        //// POST: First/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, Employee empT)
        //{
        //    try
        //    {
        //        foreach (Employee emp in employees)
        //        {
        //            if (emp.ID.Equals(id))
        //            {
        //                emp.copy(empT);
        //                return RedirectToAction("Index");
        //            }
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return RedirectToAction("Error");
        //    }
        //}

        //// GET: First/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    int i = 0;
        //    foreach (Employee emp in employees)
        //    {
        //        if (emp.ID.Equals(id))
        //        {
        //            employees.RemoveAt(i);
        //            return RedirectToAction("Details");
        //        }
        //        i++;
        //    }
        //    return RedirectToAction("Error");
        //}
    }
}
