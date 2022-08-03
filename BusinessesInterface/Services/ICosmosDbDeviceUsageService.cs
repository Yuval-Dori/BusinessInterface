namespace BusinessesInterface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessesInterface.Models;

    public interface ICosmosDbDeviceUsageService
    {
        Task<IEnumerable<Address>> GetUsageHistoryAsync(string query);
        Task<Address> GetUsageAsync(string id);
    }
}