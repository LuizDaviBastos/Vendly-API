using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ASM.Api.Controllers
{
    public class AsmBaseController : Controller
    {
        public bool TryGetSellerId(out Guid sellerId)
        {
            string? claimResult = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            return Guid.TryParse(claimResult, out sellerId);
        }
    }
}
