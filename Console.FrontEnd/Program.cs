using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Fare;
using Newtonsoft.Json;
using Shared.Models;

namespace Hurdle.FrontEnd
{
    internal class Program
    {
        private static HttpClient _client;

        private static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            MainAsync(args, cts.Token).Wait(cts.Token);
        }

        private static async Task MainAsync(string[] args, CancellationToken token)
        {
            var baseUrl = ConfigurationManager.AppSettings["baseurl"];
            _client = new HttpClient();
            using (_client)
            {
                var newIncident = new Incident
                {
                    Deadline = DateTime.UtcNow.AddDays(7),
                    Description = "Some long desc.",
                    Status = "New",
                    Title = "New incident"
                };
                var prefixGenerator = new Fare.Xeger("^[a-z][a-z0-9]{7}$");
                var randomCompanyName = prefixGenerator.Generate();

                var requestUri = String.Format("{0}/api/{1}/incident/", baseUrl, randomCompanyName);

                await OutputOperation("GET", () => _client.GetAsync(requestUri, token));
                await OutputOperation("GET", () => _client.GetAsync(requestUri + 1, token));
                await OutputOperation("PUT", () =>
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(newIncident));
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return _client.PutAsync(requestUri + 1, stringContent, token);
                });
                await OutputOperation("GET", () => _client.GetAsync(requestUri + 1, token));
                Console.WriteLine("Changing the incident status to 'in progress'");
                newIncident.Status = "In progress";
                await OutputOperation("PUT", () =>
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(newIncident));
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return _client.PutAsync(requestUri + 1, stringContent, token);
                });
                await OutputOperation("GET", () => _client.GetAsync(requestUri + 1, token));

                Console.WriteLine("Storing a bunch of incidents with...");
                for (var i = 2; i <= 20; i++)
                {
                    await OutputOperation("PUT", () =>
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(CreateIncident(i)));
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return _client.PutAsync(requestUri + i, content, token);
                    });
                }

            }
            Console.WriteLine("Looking for some Incidents");
        }

        private static Incident CreateIncident(int i)
        {
            var newIncident = new Incident
            {
                Deadline = DateTime.UtcNow.AddDays(7),
                Description = "Some long desc.",
                Status = "New",
                Title = "New incident - " + i
            };

            return newIncident;
        }

        private static async Task OutputOperation(string operation, Func<Task<HttpResponseMessage>> incidentsResponseFunc)
        {
            var incidentsResponse = await incidentsResponseFunc();
            Console.WriteLine("Did a {0} operation - Response Status Code: {1}", operation, incidentsResponse.StatusCode);

            var responseAsString = await incidentsResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Response was: {0}", responseAsString);
        }
    }
}