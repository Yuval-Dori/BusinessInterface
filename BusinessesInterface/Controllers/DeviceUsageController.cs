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
            return View(await _cosmosDbService.GetUsageHistoryAsync("SELECT * FROM c WHERE c.currentLocation= 'holon'"));
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")] //not from buisness interface, when activate create a usage object and insert to DB
        [ValidateAntiForgeryToken] // to create this in backend ? for that need to see how can get all the fields for a usage object in the back
        public async Task<ActionResult> CreateAsync([Bind("Id,DeviceId,HardwareId,CurrentLocation,Timestamp")] DeviceUsage usage)
        {
            if (ModelState.IsValid)
            {
                usage.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddUsageAsync(usage);
                return RedirectToAction("Index");
            }

            return View(usage);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("Id,DeviceId,HardwareId,CurrentLocation,TimeStamp")] DeviceUsage usage)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateUsageAsync(usage.Id, usage);
                return RedirectToAction("Index");
            }

            return View(usage);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            DeviceUsage usage = await _cosmosDbService.GetUsageAsync(id);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            DeviceUsage usage = await _cosmosDbService.GetUsageAsync(id);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await _cosmosDbService.DeleteUsageAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetUsageAsync(id));
        }
    }
}