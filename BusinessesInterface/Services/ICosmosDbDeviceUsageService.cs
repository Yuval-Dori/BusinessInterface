namespace BusinessesInterface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessesInterface.Models;

    public interface ICosmosDbDeviceUsageService
    {
        Task<Address> GetUsageHistoryAsync(string query, string socketFilter = null);
        Task<Address> GetUsageHistoryAsync(string queryString, TimeSearch searchDate);
        Task<Address> GetUsageAsync(string id);
    }
}