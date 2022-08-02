namespace BusinessesInterface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessesInterface.Models;

    public interface ICosmosDbDeviceUsageService
    {
        Task<IEnumerable<DeviceUsage>> GetUsageHistoryAsync(string query);
        Task<DeviceUsage> GetUsageAsync(string id);
        Task AddUsageAsync(DeviceUsage usage);
        Task UpdateUsageAsync(string id, DeviceUsage usage);
        Task DeleteUsageAsync(string id);
    }
}