using Microsoft.AspNetCore.Mvc;
using AFIAT.TST.Web.Controllers;
using AFIAT.TST.Pages;
using System.Threading.Tasks;

namespace AFIAT.TST.Web.Public.Controllers
{
    public class HomeController : TSTControllerBase
    {
        public IItemsAppService _itemsService;
        public HomeController(IItemsAppService itemsService)
        {
            _itemsService = itemsService;
        }
        public async Task<ActionResult> Index()
        {
            var pages = await _itemsService.GetAll(new Pages.Dtos.GetAllItemsInput { MaxResultCount = 15});
            return View(pages);
        }
        public ActionResult About()
        {
            return View();
        }
    }
}