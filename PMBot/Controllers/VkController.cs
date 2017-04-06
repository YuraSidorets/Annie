using PMBot.Helpers;
using PMBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PMBot.Controllers
{
    public class VkController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        [Route("vk/setLongPollServer")]
        public void SetLongPollServer()
        {
            try
            {
                throw new Exception("In SetLPS method");
                var response = Request.Content.ReadAsStringAsync().Result;
                // BotServices.BotService.LongPollServer = response;
                TelegramHelper.SendMessage(response);//.Response.Server);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
