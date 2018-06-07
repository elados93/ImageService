using Communication;
using ImageServiceWeb.Models;
using Infrastracture.Enums;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ConfigController : Controller
    {
        private static ConfigSingelton configModel;
        private static string m_handlerToDelete;

        public ConfigController()
        {
            configModel = ConfigSingelton.Instance;
        }

        // GET: Config
        /// <summary>
        /// when a handler was pressed, it moves us to another page that asks if we want to delet the handler.
        /// </summary>
        /// <param name="handlerToDelete"></param>
        /// <returns></returns>
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