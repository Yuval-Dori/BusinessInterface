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
           
           return View(await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.id, address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '1'"));
            
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
                var results = await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '1'", socketFilter);
                if (results == null)
                {
                    return View("NoHistoryBySocket");
                }
                return View(results);
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
                var result = await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '1'", searchDate);
                if(result == null)
                {
                    return View("NoHistoryByDate");
                }
                return View(result);
            }


            return View(date);
        }

        [ActionName("Login")]
        public IActionResult Login()
        {
            return View("LoginPage");
        }

        [HttpPost]
        [ActionName("LoginResult")]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> LoginResult(Address buisness)
        {

            if (ModelState.IsValid)
            {
                var buisnessItem = await _cosmosDbService.GetLoginDetails($"SELECT addresses.id, addresses.password FROM addresses WHERE addresses.id = '{buisness.Id}'");
                if (buisnessItem == null || !buisnessItem.Password.Equals(buisness.Password))
                {
                    return View("LoginFailed");
                }
                else if (buisnessItem.Id.Equals(buisness.Id) && buisnessItem.Password.Equals(buisness.Password))
                {
                    
                    return View("Index", await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '{buisness.Id}'"));
            
                }

                return View("LoginFailed");
            }
            return View();


        }
    }
}