using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebAPI.Backend.Models
{
    public class IncidentRepository
    {
        private static readonly string AccountName = ConfigurationManager.AppSettings["storageName"];

        private readonly StorageCredentials _cred = new StorageCredentials(AccountName,
            ConfigurationManager.AppSettings["storageKey"]);

        private readonly CloudTable _table;
        private readonly Uri _uri = new Uri(String.Format("https://{0}.table.core.windows.net/", AccountName));

        public IncidentRepository(CloudTableClient client)
        {
            if (client == null)
                client = new CloudTableClient(_uri, _cred);

            _table = client.GetTableReference("incident");
        }

        public async Task<TableResponse<IncidentDto>> GetByIdAsync(string company, string id)
        {
            await _table.CreateIfNotExistsAsync();
            var operation = TableOperation.Retrieve<IncidentDto>(company, id);
            var result = await _table.ExecuteAsync(operation);
            return new TableResponse<IncidentDto>(result);
        }

        public async Task<TableResponse<IncidentDto>> CreateAsync(IncidentDto incident)
        {
            await _table.CreateIfNotExistsAsync();
            var operation = TableOperation.Insert(incident);
            var result = await _table.ExecuteAsync(operation);
            return new TableResponse<IncidentDto>(result);
        }
    }
}