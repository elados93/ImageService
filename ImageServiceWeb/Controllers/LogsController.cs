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
            ifLogUpdate = false;
            logsModel = new LogsModel();
            logsModel.RefreshAfterUpdates += RefreshPage;
        }

        private void RefreshPage()
        {
            ifLogUpdate = true;
        }

        // GET: Logs
        public ActionResult Logs()
        {
           while(!ifLogUpdate)
            {
                Thread.Sleep(100);
            }
            return View(logsModel);
        }

        private bool ifLogUpdate;
    }
}