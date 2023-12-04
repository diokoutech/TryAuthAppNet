using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TryAuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnymousController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok("All datas anymous");
        }
    }
}
