using Microsoft.AspNetCore.Mvc;

namespace BlumindApp.Controllers {
    public class BaseController : Controller {
        public string CurrentUserId {
            get {
                return HttpContext.User.FindFirst(m => m.Type == "sub")?.Value;
            }
        }
    }
}
