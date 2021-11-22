using AFIAT.TST.Pages;
using AFIAT.TST.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AFIAT.TST.Web.Public.Controllers
{

    [Route("{controller}")]
    public class PagesController : TSTControllerBase
    {
        public IItemsAppService _itemsService;
        public PagesController(IItemsAppService itemsService)
        {
            _itemsService = itemsService;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Show(string id)
        {

            var item = await _itemsService.GetPageByTitle(id);
            if (item is null)
            {
                return NotFound();
            }
            return View(item);
        }
    }
}
