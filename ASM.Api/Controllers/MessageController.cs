using ASM.Api.Models;
using ASM.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    public class MessageController : Controller
    {
        private readonly IUnitOfWork uow;
        public MessageController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Message([FromBody] UpdateMessage updateMessage)
        {
            var seller = await uow.SellerRepository.UpdateMessage(updateMessage.Message, updateMessage.SellerId);
            return Ok(seller);
        }
    }
}
