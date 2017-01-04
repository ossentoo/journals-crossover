using System;
using System.Web.Mvc;

namespace Journals.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult RequestLengthExceeded()
        {
            return View(new HandleErrorInfo(new Exception("Uploading file this large is not supported. Please try again."), "Error", "RequestLengthExceeded"));
        }
    }
}