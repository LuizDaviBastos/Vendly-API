using ASM.Api.Helpers;
using ASM.Api.Models;
using ASM.Data.Entities;
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

        [RequestFormLimits(ValueLengthLimit = 943718400, MultipartBodyLengthLimit = 943718400)]
        [DisableRequestSizeLimit]
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromQuery] Guid messageId, [FromQuery] MessageType messageType, [FromForm(Name = "file")] IFormFile file)
        {
            if (TryGetSellerId(out Guid sellerId))
            {
                if (file == null) return BadRequest(RequestResponse.GetError("file not found"));

                Attachment attachment = new();
                string fileName = Uteis.NormalizeFileName(file.FileName, Path.GetExtension(file.FileName));
                attachment.Size = Uteis.FormatFileSize(file.Length);
                attachment.MessageId = messageId;
                attachment.Name = await storageService.Upload(file.OpenReadStream(), sellerId, messageType, fileName);
                await messageService.AddAttachment(attachment);
                return Ok(RequestResponse.GetSuccess(attachment));
            }

            return BadRequest(RequestResponse.GetError("SellerId not found"));
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetFiles(Guid messageId)
        {
            var attachments = await messageService.GetAttachments(messageId);
            return Ok(RequestResponse.GetSuccess(attachments));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFile(Guid attachmentId)
        {
            if (TryGetSellerId(out Guid sellerId))
            {
                var attachment = await messageService.DeleteAttachment(attachmentId);
                await storageService.Delete(sellerId, attachment.Message.Type, attachment.Name);
            }
            
            return Ok(RequestResponse.GetSuccess());
        }
    }
}
