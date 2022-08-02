namespace BusinessesInterface
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using BusinessesInterface.Models;

    public class CosmosDbDeviceUsageService : ICosmosDbDeviceUsageService
    {
        private Container _container;

        public CosmosDbDeviceUsageService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddUsageAsync(DeviceUsage usage)
        {
            await this._container.CreateItemAsync<DeviceUsage>(usage, new PartitionKey(usage.Id));
        }

        public async Task DeleteUsageAsync(string id)
        {
            await this._container.DeleteItemAsync<DeviceUsage>(id, new PartitionKey(id));
        }

        public async Task<DeviceUsage> GetUsageAsync(string id)
        {
            try
            {
                ItemResponse<DeviceUsage> response = await this._container.ReadItemAsync<DeviceUsage>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<DeviceUsage>> GetUsageHistoryAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<DeviceUsage>(new QueryDefinition(queryString));
            List<DeviceUsage> results = new List<DeviceUsage>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateUsageAsync(string id, DeviceUsage usage)
        {
            await this._container.UpsertItemAsync<DeviceUsage>(usage, new PartitionKey(id));
        }
    }
}