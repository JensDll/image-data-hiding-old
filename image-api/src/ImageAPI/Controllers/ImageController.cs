using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Contracts.Request;
using Domain.Enums;
using Domain.Exeptions;
using ImageAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImageController : ControllerBase
    {
        private HttpClient HttpClient { get; }
        private IDataProtectionProvider Provider { get; }
        private IEncodeService EncodeService { get; }
        private IDecodeService DecodeService { get; }

        public ImageController(IDataProtectionProvider provider,
            IEncodeService encodeService,
            IDecodeService decodeService)
        {
            HttpClient = new HttpClient();
            Provider = provider;
            EncodeService = encodeService;
            DecodeService = decodeService;
        }

        [HttpPost(ApiRoutes.ImageRoutes.EncodeFile)]
        public async Task<IActionResult> EncodeFile(IFormFile file, [FromForm] EncodeRequest request)
        {
            using var stream = new MemoryStream();

            await file.CopyToAsync(stream);

            return Ok("");
        }

        [HttpPost(ApiRoutes.ImageRoutes.EncodeRandom)]
        public async Task<IActionResult> EncodeRandom([FromBody] EncodeRequest request)
        {
            using var imageStream = await HttpClient.GetStreamAsync("https://picsum.photos/1000");

            var image = new Bitmap(imageStream);

            image.Save("C:\\Users\\jens\\original.png", ImageFormat.Png);

            try
            {
                EncodeMessage(request, image);
            }
            catch (MessageToLongExpection e)
            {
                return BadRequest(new { Error = e.Message });
            }

            using var resultStream = new MemoryStream();

            image.Save(resultStream, ImageFormat.Png);

            return File(resultStream.ToArray(), "image/png");
        }

        [HttpPost(ApiRoutes.ImageRoutes.Decode)]
        public async Task<IActionResult> Decode(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Error");
            }

            using var imageStream = new MemoryStream();

            await file.CopyToAsync(imageStream);

            var image = new Bitmap(imageStream);

            try
            {
                string message = DecodeMessage(image);

                return Ok(new { Message = message });
            }
            catch (CryptographicException e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }

        private void EncodeMessage(EncodeRequest request, Bitmap image)
        {
            var protector = Provider.CreateProtector(GetType().FullName, request.Username);
            var protectedMessage = protector.Protect(Encoding.UTF8.GetBytes(request.Message));

            EncodeService.EnocodeMessage(image, protectedMessage);
        }

        private string DecodeMessage(Bitmap image)
        {
            var protector = Provider.CreateProtector(GetType().FullName, HttpContext.GetUsername());
            var protectedMessage = DecodeService.DecodeMessage(image);
            var message = protector.Unprotect(protectedMessage);

            return Encoding.UTF8.GetString(message);
        }
    }
}
