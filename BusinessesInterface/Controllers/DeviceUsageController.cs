namespace BusinessesInterface
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using BusinessesInterface.Models;
    using Newtonsoft.Json;

    public class DeviceUsageController : Controller
    {
        private readonly ICosmosDbDeviceUsageService _cosmosDbService;

        public DeviceUsageController(ICosmosDbDeviceUsageService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index() //change query to use login id
        {
            if (HttpContext.Session.GetString("buisnessInfo") != null)
            {
                var buisnessInfo = JsonConvert.DeserializeObject<Address>(HttpContext.Session.GetString("buisnessInfo"));
                return View(await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.id, address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '{buisnessInfo.Id}'"));
            }
            return View("SignInRequest");

        }

        [ActionName("SearchSocket")]
        public IActionResult SearchSocket()
        {
            return View();
        }

        [ActionName("SearchDate")]
        public IActionResult SearchTime()
        {
            return View();
        }

        [ActionName("SearchSinceDate")]
        public async Task<IActionResult> SearchSinceDate()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ShowSocketSearchResults")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowSocketSearchResults(Device device) //change query to use login id
        {
            if (HttpContext.Session.GetString("buisnessInfo") != null)
            {
                var buisnessInfo = JsonConvert.DeserializeObject<Address>(HttpContext.Session.GetString("buisnessInfo"));
                if (device.Id == null)
                {
                    return View("NoSocketChosen");
                }
                var socketNumber = device.Id;

                if (ModelState.IsValid)
                {
                    var result = await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '{buisnessInfo.Id}'", socketNumber);
                    if (result == null)
                    {
                        return View("SocketNotExist");
                    }
                    if(result.Devices.FirstOrDefault().History.Length == 0)
                    {
                        return View("NoHistoryBySocket");
                    }
                    return View(result);
                }

                return View("NoSocketChosen");
            }
            return View("SignInRequest");

        }

        [HttpPost]
        [ActionName("ShowDateSearchResults")] //change query to use login id
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowDateSearchResults(TimeSearch date)
        {
            if (HttpContext.Session.GetString("buisnessInfo") != null)
            {
                var buisnessInfo = JsonConvert.DeserializeObject<Address>(HttpContext.Session.GetString("buisnessInfo"));
                var searchDate = date;

                if (ModelState.IsValid)
                {
                    var result = await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '{buisnessInfo.Id}'", searchDate, "specific");
                    if (result == null)
                    {
                        return View("NoHistoryByDate");
                    }
                    return View(result);
                }

                return View("NoDateChosen");
            }
            return View("SignInRequest");
        }

        [HttpPost]
        [ActionName("ShowSinceDateSearchResults")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShowSinceDateSearchResults(TimeSearch date)
        {
            if (HttpContext.Session.GetString("buisnessInfo") != null)
            {
                var buisnessInfo = JsonConvert.DeserializeObject<Address>(HttpContext.Session.GetString("buisnessInfo"));
                var searchDate = date;

                if (ModelState.IsValid)
                {
                    var result = await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '{buisnessInfo.Id}'", searchDate, "since");
                    if (result == null)
                    {
                        return View("NoHistoryByDate");
                    }
                    return View(result);
                }

                return View("NoDateChosen");
            }
            return View("SignInRequest");
        }


        [ActionName("Login")]
        public IActionResult Login()
        {
            return View("LoginPage");
        }

        [HttpPost]
        [ActionName("LoginResult")] //change query to use login id
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> LoginResult(Address buisness)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(buisness.Id))
                {
                    return View("LoginMissingFields");
                }
                var buisnessItem = await _cosmosDbService.GetLoginDetails($"SELECT addresses.id, addresses.password FROM addresses WHERE addresses.id = '{buisness.Id}'");
                if (buisnessItem == null || !buisnessItem.Password.Equals(buisness.Password))
                {
                    return View("LoginFailed");
                }
                else if (buisnessItem.Id.Equals(buisness.Id) && buisnessItem.Password.Equals(buisness.Password))
                {
                    HttpContext.Session.SetString("buisnessInfo", JsonConvert.SerializeObject(buisness));
                    return View("Index", await _cosmosDbService.GetUsageHistoryAsync($"SELECT address.devices FROM addresses address JOIN device IN address.devices WHERE address.id = '{buisness.Id}'"));
            
                }

                return View("LoginFailed");
            }
            return View("LoginFailed");


        }
    }
}