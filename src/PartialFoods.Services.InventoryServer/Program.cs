using System;
using System.IO;
using System.Threading;
using Grpc.Core;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PartialFoods.Services;
using PartialFoods.Services.InventoryServer.Entities;

namespace PartialFoods.Services.InventoryServer
{
    class Program
    {
        private static ManualResetEvent mre = new ManualResetEvent(false);

        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables();

            Configuration = builder.Build();

            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();

            ILogger logger = loggerFactory.CreateLogger<Program>();

            var port = int.Parse(Configuration["service:port"]);

            string brokerList = Configuration["kafkaclient:brokerlist"];
            const string reservedTopic = "inventoryreserved";
            const string releasedTopic = "inventoryreleased";

            var config = new Dictionary<string, object>
            {
                { "group.id", "inventory-server" },
                { "enable.auto.commit", false },
                { "bootstrap.servers", brokerList }
            };
            var context = new InventoryContext(Configuration["postgres:connectionstring"]);
            var repo = new InventoryRepository(context);

            var reservedEventProcessor = new InventoryReservedEventProcessor(repo);
            var kafkaConsumer = new KafkaReservedConsumer(reservedTopic, config, reservedEventProcessor);
            kafkaConsumer.Consume();

            var releasedEventProcessor = new InventoryReleasedEventProcessor(repo);
            var releasedConsumer = new KafkaReleasedConsumer(releasedTopic, config, releasedEventProcessor);
            releasedConsumer.Consume();

            Server server = new Server
            {
                Services = { InventoryManagement.BindService(new InventoryManagementImpl(repo)) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Start();
            logger.LogInformation("Inventory RPC Service Listening on Port " + port);

            mre.WaitOne();
        }
    }
}
