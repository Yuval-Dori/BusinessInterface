namespace BusinessesInterface
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using BusinessesInterface.Models;

    public class DeviceUsageController : Controller
    {
        private readonly ICosmosDbDeviceUsageService _cosmosDbService;

        public DeviceUsageController(ICosmosDbDeviceUsageService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetUsageHistoryAsync("SELECT address.id, address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '1'"));
        }

        [ActionName("SearchSocket")]
        public IActionResult SearchSocket()
        {
            return View();
        }

        [ActionName("SearchTime")]
        public IActionResult SearchTime()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ShowSearchResults")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowSearchResults(Device dev)
        {
            var socketFilter = dev.Id;

            if (ModelState.IsValid)
            {
                return View(await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '1'", socketFilter));
            }
      

            return View(dev);
        }

        [HttpPost]
        [ActionName("ShowDateSearchResults")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowDateSearchResults(TimeSearch date)
        {
            var searchDate = date;
            
            if (ModelState.IsValid)
            {
                return View(await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '1'", searchDate));
            }


            return View(date);
        }

        [ActionName("LoginPage")]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        [ActionName("LoginResult")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginResult(Address buisness)
        {

            if (ModelState.IsValid)
            {
                return View(await _cosmosDbService.GetLoginDetails($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '1'", searchDate));
            }


            return View(date);
        }


    }
}