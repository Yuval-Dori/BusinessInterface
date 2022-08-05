namespace BusinessesInterface
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using BusinessesInterface.Models;
    using System.Globalization;

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

        public async Task<Address> GetUsageAsync(string id)
        {
            try
            {
                ItemResponse<Address> response = await this._container.ReadItemAsync<Address>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Device>> GetUsageHistoryAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Device>(new QueryDefinition(queryString));
            List<Device> results = new List<Device>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                var responseList = response.ToList();

                DateTime dateTime;
                foreach (var item in responseList)
                {
                    var tsList = new List<DateTime>();
                    foreach(var stamp in item.History)
                    {
                        dateTime = DateTimeOffset.FromUnixTimeSeconds(stamp).DateTime;
                        tsList.Add(dateTime);
                    }

                    item.HistoryDateTime = tsList.ToArray();
                }

                results.AddRange(responseList);
            }

            return results;
        }
    }
}