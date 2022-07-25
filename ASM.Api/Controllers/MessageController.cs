using ASM.Api.Models;
using ASM.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        [HttpPost("SaveMessage")]
        public async Task<IActionResult> Message([FromBody] UpdateMessage updateMessage)
        {
            var seller = await uow.SellerRepository.UpdateMessage(updateMessage.Message, updateMessage.SellerId);
            await uow.CommitAsync();

            if (seller == null)
            {
                return BadRequest($"Seller not found with id: {updateMessage.SellerId}");
            }
            return Ok(seller);
        }

        [HttpGet("GetMessage")]
        public IActionResult Message(GetMessage getMessage)
        {
            var seller = uow.SellerRepository.GetBySellerId(getMessage.SellerId);
            return Ok(new { Message = seller.Message });
        }
    }
}
