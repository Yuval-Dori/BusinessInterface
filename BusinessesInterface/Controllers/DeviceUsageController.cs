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
            return View(await _cosmosDbService.GetUsageHistoryAsync("SELECT device.id, device.history FROM addresses address JOIN device IN address.devices WHERE address.id = '1'"));
        }

        [ActionName("SearchSocket")]
        public IActionResult SearchSocket()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ShowSearchResults")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowSearchResults(Device dev)
        {
            if (ModelState.IsValid)
            {
                return View(await _cosmosDbService.GetUsageHistoryAsync($"SELECT device.id, device.history FROM addresses address JOIN device IN address.devices WHERE address.id = '1' AND device.id ='{dev.Id}'"));
            }

            return View(dev);
        }

    }
}