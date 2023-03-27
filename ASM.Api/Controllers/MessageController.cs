using ASM.Api.Helpers;
using ASM.Api.Models;
using ASM.Data.Entities;
using ASM.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
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
            var seller = uow.SellerRepository.UpdateMessage(updateMessage);
            await uow.CommitAsync();
            return Ok(seller);
        }

        [HttpGet("Get")]
        public IActionResult Message(long meliSellerId)
        {
            var seller = uow.SellerRepository.GetByMeliSellerId(meliSellerId);
            return Ok(seller);
        }
    }
}
