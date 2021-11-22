using Abp.Application.Services.Dto;
using AFIAT.TST.Pages;
using AFIAT.TST.Pages.Dtos;
using AFIAT.TST.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AFIAT.TST.Web.Public.Controllers
{
    [Route("{controller}")]
    public class UploadController : TSTControllerBase
    {
        private IWebHostEnvironment _environment;
        public IItemsAppService _itemsService;
        public UploadController(IWebHostEnvironment environment, IItemsAppService itemsService)
        {
            _environment = environment;
            _itemsService = itemsService;
        }
        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile file,[FromQuery] int itemId)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "images");
            Directory.CreateDirectory(uploads);
            if (file.Length > 0)
            {
                var filePath = Path.Combine(uploads, file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                var item = await _itemsService.GetItemForEdit(new EntityDto { Id = itemId });
                if (item is not null)
                {
                    item.Item.ImageAdress = "images/"+ file.FileName;        
                    await _itemsService.CreateOrEdit(item.Item);
                    return Ok(new { path = item.Item.ImageAdress });
                }
      
            }
            return BadRequest();
        }
    }
}
