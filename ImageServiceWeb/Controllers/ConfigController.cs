using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        private static ConfigModel configModel = new ConfigModel();
        private static string m_handlerToDelete;

        public ConfigController() {}

        // GET: Config
        public ActionResult OnHandlerDeletion(string handlerToDelete)
        {
            m_handlerToDelete = handlerToDelete;
            return RedirectToAction("CheckDeletion");
        }

        public ActionResult Config()
        {
            return View(configModel);
        }

        public ActionResult CheckDeletion()
        {
            return View(configModel);
        }

        public ActionResult DeleteOK()
        {
            configModel.RemoveHandler(m_handlerToDelete);
            return RedirectToAction("Config");
        }

        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }
    }
}