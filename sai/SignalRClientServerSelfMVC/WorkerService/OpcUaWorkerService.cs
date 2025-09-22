
using Microsoft.AspNetCore.SignalR;
using Opc.UaFx;
using Opc.UaFx.Client;
using SignalRClientServerSelfMVC.Models;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Text.Json;

namespace SignalRClientServerSelfMVC.WorkerService
{
    public class OpcUaWorkerService : BackgroundService
    {
        private readonly ILogger<OpcUaWorkerService> _logger;
        private readonly IHubContext<ChatHub> hubContext;

        public OpcUaWorkerService(ILogger<OpcUaWorkerService> logger, IHubContext<ChatHub> hubContext)
        {
            this._logger = logger;
            this.hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            subscription();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public async void subscription()
        {
            _logger.LogInformation("Subscription started " + DateTime.Now.ToString());

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


                    }

                    // Apply the changes to activate the subscription
                    subscription.ApplyChanges();

                    await Task.Delay(Timeout.Infinite);

                }
                catch (Exception ex)
                {
                    // Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            _logger.LogInformation("Subscription completed " + DateTime.Now.ToString());

        }

        // Event handler for data changes
        async void HandleDataChange(object sender, OpcDataChangeReceivedEventArgs e)
        {
            try
            {
                OpcMonitoredItem item = (OpcMonitoredItem)sender;
                _logger.LogInformation($"Data Change from NodeId '{item.NodeId.Value}': {e.Item.Value} {e.Item.Notification.PublishTime}");

                await sendSigalRData(item.NodeId.Value.ToString(), Convert.ToInt16(e.Item.Value.Value));

            }
            catch (Exception ex)
            {

            }

        }

        public async Task<bool> sendSigalRData(string tagName, int tagValue)
        {
            _logger.LogInformation("sendSigalRData start");
            ReceivedOpcData opcData = new ReceivedOpcData(tagName, tagValue);
            await hubContext.Clients.All.SendAsync("OpcUaReceiveMessage", JsonSerializer.Serialize(opcData).ToString());
            _logger.LogInformation("sendSigalRData end");
            return true;
        }
    }
}
