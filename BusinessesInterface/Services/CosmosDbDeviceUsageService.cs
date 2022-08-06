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

        public async Task<Address> GetUsageHistoryAsync(string queryString, string socketFilter = null)
        {
            var query = this._container.GetItemQueryIterator<Address>(new QueryDefinition(queryString));

            var results = new Address();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                var item = response.ToList()[0];
   
                DateTime dateTime;
               
                foreach(var dev in item.Devices)
                {
                    if(socketFilter != null && !dev.Id.Equals(socketFilter))
                    {
                        continue;
                    }
                    var tsList = new List<DateTime>();
                    foreach (var stamp in dev.History)
                    {
                        dateTime = DateTimeOffset.FromUnixTimeSeconds(stamp).DateTime;
                        tsList.Add(dateTime);
                    }
                        dev.HistoryDateTime = tsList.ToArray();
                }
                results = item;
            }

            return results;
        }

        public async Task<Address> GetUsageHistoryAsync(string queryString, TimeSearch searchDate)
        {
            var query = this._container.GetItemQueryIterator<Address>(new QueryDefinition(queryString));

            var results = new Address();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                var item = response.ToList()[0];
           
                DateTime dateTime;

                foreach (var dev in item.Devices)
                {
                    var tsList = new List<DateTime>();
                    foreach (var stamp in dev.History)
                    {
                        dateTime = DateTimeOffset.FromUnixTimeSeconds(stamp).DateTime;
                        var dateTimeDay = dateTime.Date;
                        var dateSearchDate = searchDate.TimeToSearch.Date;
                        if (dateSearchDate == dateTimeDay)
                        {
                            tsList.Add(dateTime);
                        }
                        else
                        {
                            continue;
                        }
                       
                    }
                    dev.HistoryDateTime = tsList.ToArray();
                }
                results = item;
            }

            return results;
        }


    }
}