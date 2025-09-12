
using Opc.UaFx;
using Opc.UaFx.Client;

namespace WorkerServiceMicroservice
{
    public class OPCUAWorkerService : BackgroundService
    {
        private readonly ILogger<OPCUAWorkerService> logger;

        public OPCUAWorkerService(ILogger<OPCUAWorkerService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            subscription();
            while (!stoppingToken.IsCancellationRequested)
            {
                // logger.LogInformation("OPC worker running");

                //string random= new Random().Next(1,1000).ToString();
                //writeTag("ns=2;s=Channel1.Device1.Station2", random);
                await Task.Delay(5000, stoppingToken);
            }
        }

        public string readTag(string tagName)
        {
            string opcUrl = "opc.tcp://CTHPGK3:49320";
            //var tagName = "ns=2;s=Channel1.Device1.Station2";
            var client = new OpcClient(opcUrl);
            client.Connect();
            string temp = client.ReadNode(tagName).ToString();
            client.Disconnect();
            return temp;
        }

        public void writeTag(string tagName, string value)
        {
            string opcUrl = "opc.tcp://CTHPGK3:49320";
            //var tagName = "ns=2;s=Channel1.Device1.Station2";
            var client = new OpcClient(opcUrl);
            client.Connect();
            client.WriteNode(tagName, value);
            client.Disconnect();
        }

        public void subscription()
        {
            // Replace with your OPC UA server URL
            string serverUrl = "opc.tcp://CTHPGK3:49320";

            // Replace with the NodeId you want to subscribe to (e.g., "ns=2;s=Message")
            string nodeIdToSubscribe = "ns=2;s=Channel1.Device1.Station2";

            using (var client = new OpcClient(serverUrl))
            {
                try
                {
                    client.Connect();
                    logger.LogInformation($"Connected to OPC UA server at {serverUrl}");

                    // Create an OpcSubscription object
                    OpcSubscription subscription = client.SubscribeNodes();

                    // Create an OpcMonitoredItem for the desired node
                    var monitoredItem = new OpcMonitoredItem(nodeIdToSubscribe, OpcAttribute.Value);

                    // Attach an event handler for data changes
                    monitoredItem.DataChangeReceived += HandleDataChange;

                    // Set sampling interval (optional, in milliseconds)
                    monitoredItem.SamplingInterval = 1000;

                    // Add the monitored item to the subscription
                    subscription.AddMonitoredItem(monitoredItem);

                    // Apply the changes to activate the subscription
                    subscription.ApplyChanges();

                    logger.LogInformation($"Subscribed to NodeId: {nodeIdToSubscribe}. Press any key to exit.");
                    // Console.ReadKey();

                    // Unsubscribe (optional, automatically done on client dispose)
                    //subscription.RemoveMonitoredItem(monitoredItem);
                    //subscription.ApplyChanges();

                    logger.LogInformation("Unsubscribed and disconnected.");
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"An error occurred: {ex.Message}");
                }
            }
        }

        // Event handler for data changes
        private void HandleDataChange(object sender, OpcDataChangeReceivedEventArgs e)
        {
            OpcMonitoredItem item = (OpcMonitoredItem)sender;
            logger.LogInformation($"Data Change from NodeId '{item.DisplayName}': {e.Item.Value}");
        }


    }
}
