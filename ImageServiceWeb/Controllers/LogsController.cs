using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            logsModel.RefreshAfterUpdates += RefreshPage;
        }

        private ActionResult RefreshPage()
        {
            return View(logsModel);
        }

        // GET: Logs
        public ActionResult Logs()
        {
            return View(logsModel);
        }
    }
}