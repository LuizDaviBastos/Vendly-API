using AfterSalesMessage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AfterSalesMessageApi.Controllers
{
    [Route("api/[controller]")]
    public class SellerTriggerController : ControllerBase
    {

        [HttpGet("SellerTriggerGet")]
        public async Task SellerTrigger()
        {
            //send to queue
            var storage = new StorageService();
            await storage.SendMessage("", new object { });
        }

        [HttpPost("SellerTriggerPost")]
        public async Task SellerTrigger(object body)
        {
            //send to queue
            var storage = new StorageService();
            await storage.SendMessage("", new object { });
        }
        //ngrok http -host-header=localhost https://localhost:7024/
    }
}
