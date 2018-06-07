using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class LogsController : Controller
    {
        private static LogsModel logsModel;

        public LogsController()
        {
            logsModel = new LogsModel();
        }

        public ActionResult Logs()
        {
            return View(logsModel.Logs);
        }

        [HttpPost]
        public ActionResult Logs(FormCollection form)
        {
            string type = form["filterMessages"].ToString();
            if (type == "")
            {
                return View(logsModel.Logs);
            }
            List<EntryLog> filteredLogsList = new List<EntryLog>();
            foreach (EntryLog log in logsModel.Logs)
            {
                if (log.EntryType == type)
                {
                    filteredLogsList.Add(log);
                }
            }
            return View(filteredLogsList);
        }
    }
}