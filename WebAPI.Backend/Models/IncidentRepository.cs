using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebAPI.Backend.Models
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly CloudTable _table;

        public IncidentRepository(CloudTableClient client)
        {
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
            return await ExecuteOperationAsync(incident, TableOperation.Insert);
        }

        public async Task<TableResponse<IncidentDto>> UpdateAsync(IncidentDto incident)
        {
            return await ExecuteOperationAsync(incident, TableOperation.Merge);
        }

        private async Task<TableResponse<IncidentDto>> ExecuteOperationAsync(ITableEntity incident,
            Func<ITableEntity, TableOperation> operation)
        {
            try
            {
                await _table.CreateIfNotExistsAsync();
                var oper = operation(incident);
                var result = await _table.ExecuteAsync(oper);
                return new TableResponse<IncidentDto>(result);
            }
            catch (StorageException ex)
            {
                var tableResult = new TableResult {HttpStatusCode = ex.RequestInformation.HttpStatusCode};
                return new TableResponse<IncidentDto>(tableResult);
            }
        }
    }
}