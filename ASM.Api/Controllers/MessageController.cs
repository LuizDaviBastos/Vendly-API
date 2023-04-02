using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
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
        private readonly IUnitOfWork uow;
        public MessageController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Message([FromBody] SellerMessage updateMessage)
        {
            uow.MessageRepository.AddOrUpdate(updateMessage);
            await uow.CommitAsync();
            return Ok(RequestResponse.GetSuccess(updateMessage));
        }

        [HttpGet("Get")]
        public IActionResult Message(Guid meliAccountId, MessageType messageType)
        {
            var message = uow.MessageRepository.GetMessage(meliAccountId, messageType);
            return Ok(RequestResponse.GetSuccess(message));
        }
    }
}
