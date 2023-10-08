using Microsoft.AspNetCore.Mvc;
using RegistrantApplication.Server.Database;

namespace RegistrantApplication.Server.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly LiteContext _ef;
        public BaseController(ILogger<BaseController> logger, LiteContext ef)
        {
            _logger = logger;
            _ef = ef;
        }
    }
}
