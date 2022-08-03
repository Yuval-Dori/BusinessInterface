﻿namespace BusinessesInterface
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
            return View(await _cosmosDbService.GetUsageHistoryAsync("SELECT * FROM c"));
        }

    }
}