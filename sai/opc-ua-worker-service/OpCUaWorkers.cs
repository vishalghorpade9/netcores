using Microsoft.Extensions.Logging;
using Opc.Ua.Client;
using Opc.UaFx;
using Opc.UaFx.Client;
using opc_ua_worker_service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace opc_ua_worker_service
{
    public class OpCUaWorkers : BackgroundService
    {
        private readonly ILogger<OpCUaWorkers> _logger;
        private readonly IServiceScopeFactory scopeFactory;

        private static string logFilePath = "C:\\Users\\VGhorpade\\source\\repos\\netcores\\sai\\opc-ua-worker-service\\application_log.txt";

        public OpCUaWorkers(ILogger<OpCUaWorkers> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            createLogs("Worker service started");
            subscription();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("OpCUaWorkers running " + DateTime.Now.ToString());
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public async void subscription()
        {
            _logger.LogInformation("Subscription started " + DateTime.Now.ToString());
            createLogs("Subscription started " + DateTime.Now.ToString());
            // Replace with your OPC UA server URL
            string serverUrl = "opc.tcp://CTHPGK3:49320";

            // Replace with the NodeId you want to subscribe to (e.g., "ns=2;s=Message")
            // string nodeIdToSubscribe = "ns=2;s=Channel1.Device1.Station1Call";

            string[] machineStatusTag = ["ns=2;s=Channel2.Device1.station1",
    "ns=2;s=Channel2.Device1.station2",
    "ns=2;s=Channel2.Device1.station3",
    "ns=2;s=Channel2.Device1.station4"];

            OpcMonitoredItem[] opcMonitoredItems = new OpcMonitoredItem[machineStatusTag.Length];

            using (var client = new OpcClient(serverUrl))
            {
                try
                {
                    client.Connect();
                    Console.WriteLine($"Connected to OPC UA server at {serverUrl}");
                    createLogs($"Connected to OPC UA server at {serverUrl}");

                    // Create an OpcSubscription object
                    OpcSubscription subscription = client.SubscribeNodes();

                    for (int i = 0; i < machineStatusTag.Length; i++)
                    {
                        var monitoredItem = new OpcMonitoredItem(machineStatusTag[i], OpcAttribute.Value);

                        // Attach an event handler for data changes
                        monitoredItem.DataChangeReceived += HandleDataChange;

                        // Set sampling interval (optional, in milliseconds)
                        monitoredItem.SamplingInterval = 1000;

                        // Add the monitored item to the subscription
                        subscription.AddMonitoredItem(monitoredItem);

                        opcMonitoredItems[i] = monitoredItem;

                        createLogs($"Subscribed to NodeId: {machineStatusTag[i]}");
                    }

                    // Apply the changes to activate the subscription
                    subscription.ApplyChanges();

                    await Task.Delay(Timeout.Infinite);

                    //Console.WriteLine($"Subscribed to NodeId: {nodeIdToSubscribe}. Press any key to exit.");
                    //Console.ReadKey();

                    //for (int i = 0; i < opcMonitoredItems.Count(); i++)
                    //{
                    //    // Unsubscribe (optional, automatically done on client dispose)
                    //    subscription.RemoveMonitoredItem(opcMonitoredItems[i]);
                    //}

                    //// Unsubscribe (optional, automatically done on client dispose)
                    ////    subscription.RemoveMonitoredItem(monitoredItem);
                    ////subscription.RemoveMonitoredItem(monitoredItem1);
                    //subscription.ApplyChanges();

                    //Console.WriteLine("Unsubscribed and disconnected.");
                }
                catch (Exception ex)
                {
                    createLogs($"An error occurred: {ex.Message}");
                    // Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            _logger.LogInformation("Subscription completed " + DateTime.Now.ToString());
            createLogs("Subscription completed " + DateTime.Now.ToString());
        }

        // Event handler for data changes
        void HandleDataChange(object sender, OpcDataChangeReceivedEventArgs e)
        {
            try
            {

                createLogs("Data change event fired");
                OpcMonitoredItem item = (OpcMonitoredItem)sender;
                _logger.LogInformation($"Data Change from NodeId '{item.NodeId.Value}': {e.Item.Value} {e.Item.Notification.PublishTime}");
                createLogs($"Data Change from NodeId '{item.NodeId.Value}': {e.Item.Value} {e.Item.Notification.PublishTime}");

                using (var context = new DatabaseContext())
                {
                    var tagName = item.NodeId.Value;

                    var machines = context.Machines.Where(p => p.MachineStatusTag.Equals(tagName)).FirstOrDefault();
                    if (machines != null)
                    {
                        machines.MachineStatus = Convert.ToInt16(e.Item.Value.Value);
                        context.SaveChanges();
                    }
                }

                //var scope = scopeFactory.CreateScope();

                //using (var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
                //{
                //    var tagName = item.NodeId.Value;

                //    var machines = context.Machines.Where(p => p.MachineStatusTag.Equals(tagName)).FirstOrDefault();
                //    if (machines != null)
                //    {
                //        machines.MachineStatus = Convert.ToInt16(e.Item.Value.Value);
                //        context.SaveChanges();
                //    }
                //}

            }
            catch (Exception ex)
            {
                createLogs(ex.Message);
            }

        }

        void createLogs(string log)
        {
            // Ensure the directory exists (optional, if you're not using a specific subfolder)
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Format the log entry with a timestamp
            string logEntry = $"{DateTime.Now}: {log}";

            // Append the log entry to the file. If the file doesn't exist, it will be created.
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
    }
}
