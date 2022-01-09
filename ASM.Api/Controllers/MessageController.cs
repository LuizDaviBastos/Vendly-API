using ASM.Api.Models;
using ASM.Core.Repositories;
using ASM.Data.Entities;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    public class MessageController : Controller
    {
                private readonly ISellerRepository sellerRepository;
        public MessageController(ISellerRepository sellerRepository)
        {
            this.sellerRepository = sellerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Message([FromBody] UpdateMessage updateMessage)
        {
           var seller = await sellerRepository.UpdateMessage(updateMessage.Message, updateMessage.SellerId);
            return Ok(seller);
        }
    }
}
