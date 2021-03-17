using Application.Common;
using Application.Common.Interfaces.Services;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Domain.Exceptions;
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
        public async Task<IActionResult> EncodeFile([FromForm] EncodeRequest request, IFormFile file)
        {
            using var imageStream = new MemoryStream();

            await file.CopyToAsync(imageStream);
            var image = new Bitmap(imageStream);

            return EncodeRequest(request, image);
        }

        [HttpPost(ApiRoutes.ImageRoutes.EncodeRandom)]
        public async Task<IActionResult> EncodeRandom([FromBody] EncodeRequest request)
        {
            using var imageStream = await HttpClient.GetStreamAsync("https://picsum.photos/1000");

            var image = new Bitmap(imageStream);

            return EncodeRequest(request, image);
        }

        [HttpPost(ApiRoutes.ImageRoutes.Decode)]
        [ProducesResponseType(typeof(DecodeResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
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

                return Ok(new DecodeResponse
                {
                    Message = message
                });
            }
            catch (CryptographicException e)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessages = new[] { e.Message }
                });
            }
        }

        private IActionResult EncodeRequest(EncodeRequest request, Bitmap image)
        {
            try
            {
                EncodeMessage(request, image);
            }
            catch (MessageToLongException e)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessages = new[] { e.Message }
                });
            }

            using var resultStream = new MemoryStream();

            image.Save(resultStream, ImageFormat.Png);

            return File(resultStream.ToArray(), "image/png");
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
