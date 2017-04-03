using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using PMBot.BotServices;

namespace PMBot.Controllers
{
    public class BotController : ApiController
    {
        [HttpPost]
        public OkResult Run()
        {
          
            return Ok();
        }
    }
}
