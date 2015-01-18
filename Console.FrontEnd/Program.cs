using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConsoleMenu;
using Shared.Models;

namespace Hurdle.FrontEnd
{
    internal class Program
    {
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
            var menu = MenuHelper.GetOperations();

            var operation = menu.Display();
            if (operation.IsExit) return;

            Console.WriteLine("What company to use?");
            var company = Console.ReadLine();

            Console.WriteLine("What id to use?");
            var id = Console.ReadLine();

            if (operation.RequiresBody)
            {
                var incident = new Incident();
                Console.WriteLine("Incident Title?");
                incident.Title = Console.ReadLine();

                Console.WriteLine("Incident Description?");
                incident.Description = Console.ReadLine();

                Console.WriteLine("Incident Status?");
                incident.Status = Console.ReadLine();

                Console.WriteLine("Incident Deadline?");
                var deadline = Console.ReadLine();
                incident.Deadline = DateTime.Parse(deadline);
            }

        }
    }

    public class Operation
    {
        public string Name { get; set; }
        public bool IsExit { get; set; }
        public bool RequiresBody { get; set; }

    }

    public static class MenuHelper
    {
        public static TypedMenu<Operation> GetOperations()
        {
            var ops = new List<Operation>
            {
                new Operation {Name = "Get"},
                new Operation {Name = "Post", RequiresBody = true},
                new Operation {Name = "Patch", RequiresBody = true},
                new Operation {Name = "Exit", IsExit = true}
            };

            return new TypedMenu<Operation>(ops, "Pick an operation", operation => operation.Name);
        }
    }
}