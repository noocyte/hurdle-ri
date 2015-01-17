using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebAPI.Backend.Models
{
    public class IncidentRepository
    {
        private StorageCredentials cred = new StorageCredentials("uxhurdle",
            "FdLMeczIrFo7Jtcoad1GL6AFYS5abqU2G53T3KapK/pbS1lRzNMq7jZdz8ziZy2ScewEzRn66w0/NnkzbZehzA==");

        StorageUri uri = new StorageUri(new Uri("https://uxhurdle.table.core.windows.net/"));

        public IncidentRepository(CloudTableClient client)
        {
            if (client == null)
                client = new CloudTableClient(uri, cred);

            var table = client.GetTableReference("incident");
        }
    }
}