﻿using Communication;
using ImageServiceWeb.Models;
using Infrastracture.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        private static ConfigModel configModel;
        private static string m_handlerToDelete;

        public ConfigController()
        {
            configModel = new ConfigModel();
            configModel.RefreshAfterUpdates += RefreshPage;
        }

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
            //configModel.RemoveHandler(m_handlerToDelete);
            string[] args = new string[1];
            args[0] = m_handlerToDelete;
            MessageCommand removeHandler = new MessageCommand((int)CommandEnum.CloseCommand, args, m_handlerToDelete);
            configModel.imageServiceClient.sendCommand(removeHandler);
            return RedirectToAction("Config");
        }

        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }

        private ActionResult RefreshPage()
        {
            return View(configModel);
        }
    }
}