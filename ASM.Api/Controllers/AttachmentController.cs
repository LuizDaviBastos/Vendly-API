using ASM.Api.Helpers;
using ASM.Api.Models;
using ASM.Data.Enums;
using ASM.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AttachmentController : AsmBaseController
    {
        private readonly IStorageService storageService;
        private readonly IMessageService messageService;
        public AttachmentController(IStorageService storageService, IMessageService messageService)
        {
            this.storageService = storageService;
            this.messageService = messageService;
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] IFormFile file)
        {
            if (TryGetSellerId(out Guid sellerId))
            {                
                await storageService.Upload(file.OpenReadStream(), sellerId, Uteis.NormalizeFileName(file.FileName, Path.GetExtension(file.FileName)));
                return Ok(RequestResponse.GetSuccess());
            }

            return BadRequest(RequestResponse.GetError("SellerId not found"));
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetFiles(Guid sellerId)
        {
            var attachments = await messageService.GetAttachments(sellerId);
            return Ok(RequestResponse.GetSuccess(attachments));
        }
    }
}
