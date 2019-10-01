using Microsoft.AspNetCore.Mvc;

namespace BlumindApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase {
        public string CurrentUserId {
            get {
                return HttpContext.User.FindFirst(m => m.Type == "sub")?.Value;
            }
        }
    }
}
