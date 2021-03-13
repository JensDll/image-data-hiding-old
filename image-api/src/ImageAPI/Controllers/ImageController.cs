using Application.Services;
using Domain;
using Domain.Contracts.Request;
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
using System.Text;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImageController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IDataProtectionProvider _provider;
        private readonly IImageService _imageService;

        public ImageController(IDataProtectionProvider provider, IImageService imageService)
        {
            _client = new HttpClient();
            _provider = provider;
            _imageService = imageService;
        }

        [HttpPost(ApiRoutes.ImageRoutes.Encode)]
        public async Task<IActionResult> Encode(EncodeRequest request)
        {
            var imagestream = await _client.GetStreamAsync("https://picsum.photos/300/400");

            var (userId, message) = request;
            var protector = _provider.CreateProtector(GetType().ToString(), userId.ToString());

            var protectedData = protector.Protect(Encoding.UTF8.GetBytes(message));

            var image = new Bitmap(imagestream);

            _imageService.WriteMessage(image, protectedData);

            var resultStream = new MemoryStream();

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

            var stream = new MemoryStream();

            await file.CopyToAsync(stream);

            var image = new Bitmap(stream);

            _imageService.ReadMessage(image);

            var protector = _provider.CreateProtector(GetType().ToString(), HttpContext.GetUserId().ToString());

            return Ok(new
            {
                Message = "Test"
            });
        }

        [HttpGet(ApiRoutes.ImageRoutes.Test)]
        public IActionResult Test(string s)
        {
            int id = HttpContext.GetUserId();

            return Ok(id);
        }
    }
}
