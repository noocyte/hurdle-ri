using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Ninject.Modules;
using WebAPI.Backend.Models;

namespace WebAPI.Backend.Modules
{
    public class ApiModule : NinjectModule
    {
        private static readonly string AccountName = ConfigurationManager.AppSettings["storageName"];

        private readonly StorageCredentials _cred = new StorageCredentials(AccountName,
            ConfigurationManager.AppSettings["storageKey"]);

        private readonly Uri _uri = new Uri(String.Format("https://{0}.table.core.windows.net/", AccountName));

        public override void Load()
        {
            Bind<IIncidentRepository>().To<IncidentRepository>();
            
            var client = new CloudTableClient(_uri, _cred);
            Bind<CloudTableClient>().ToConstant(client);
        }
    }
}