using System;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace WebAPI.Backend.Models
{
    public class IncidentDto : TableEntity
    {
        public IncidentDto()
        {
        }

        public IncidentDto(string company, string id)
        {
            PartitionKey = company;
            RowKey = id;
        }

        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("deadline")]
        public DateTime Deadline { get; set; }
    }
}