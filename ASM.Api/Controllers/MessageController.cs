using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService messageService;
        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Message([FromBody] SellerMessage updateMessage)
        {
            await messageService.UpdateMessage(updateMessage);
            return Ok(RequestResponse.GetSuccess(updateMessage));
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Message(Guid meliAccountId, MessageType messageType)
        {
            var message = await messageService.GetMessage(meliAccountId, messageType);
            return Ok(RequestResponse.GetSuccess(message));
        }
    }
}
