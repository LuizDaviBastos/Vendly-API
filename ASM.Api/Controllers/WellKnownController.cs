using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellKnownController : ControllerBase
    {
        public WellKnownController()
        {

        }
        [Route(".well-known/assetlinks.json")]
        public ContentResult AppleMerchantIDDomainAssociation()
        {
            return new ContentResult
            {
                Content = @"[{
                              ""relation"": [""delegate_permission/common.handle_all_urls""],
                              ""target"" : { ""namespace"": ""android_app"", ""package_name"": ""io.ionic.starter"",
                                           ""sha256_cert_fingerprints"": [""53:3D:D7:A2:81:60:AA:0E:03:BF:C5:7B:C4:2A:58:34:36:08:5C:D3:02:6C:9A:86:86:EC:D0:2B:CD:9E:28:D5""] }
                                 }]",
                ContentType = "application/json"
            };
        }
    }
}
