using ASM.Api.Helpers;
using ASM.Api.Models;
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
        public async Task<IActionResult> Message([FromBody] UpdateMessage updateMessage)
        {
            var seller = uow.SellerRepository.UpdateMessage(updateMessage.ToEntity());
            await uow.CommitAsync();

            if (seller == null)
            {
                return BadRequest($"Seller not found with id: {updateMessage.SellerId}");
            }
            return Ok(seller);
        }

        [HttpGet("Get")]
        public IActionResult Message(long sellerId)
        {
            var seller = uow.SellerRepository.GetBySellerId(sellerId);
            return Ok(seller.ToUpdateMessage());
        }
    }
}
