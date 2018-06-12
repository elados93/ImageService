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

        /// <summary>
        /// in case of chosing the config view,leads to the config page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            return View(configModel);
        }


        /// <summary>
        /// Checks the deletion by sending the user to the relevent page
        /// that asks him whther he wants to delete the photo that he preesed on.
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckDeletion()
        {
            return View(configModel);
        }


        /// <summary>
        /// in case the user want to delete the handler, sending to the config
        /// model to remove the specific handler from the list
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteOK()
        {
            configModel.RemoveHandler(m_handlerToDelete);
            return RedirectToAction("Config");
        }


        /// <summary>
        /// in case the user dont want to delete the handler return to the config page
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteCancel()
        {
            return RedirectToAction("Config");
        }
    }
}