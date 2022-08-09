namespace BusinessesInterface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessesInterface.Models;

    public interface ICosmosDbDeviceUsageService
    {
        Task<Address> GetUsageHistoryAsync(string query, string socketNumber = null);
        Task<Address> GetUsageHistoryAsync(string queryString, TimeSearch searchDate, string searchFor);
        Task<Address> GetLoginDetails(string querystring);
    }
}