using Opc.UaFx;
using Opc.UaFx.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OPCUA_Subscriptions
{
    internal class Program
    {
        static void Main(string[] args)
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
                    Console.WriteLine($"Connected to OPC UA server at {serverUrl}");

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


                    // Create an OpcMonitoredItem for the desired node
                    var monitoredItem1 = new OpcMonitoredItem("ns=2;s=Channel1.Device1.Station5", OpcAttribute.Value);

                    // Attach an event handler for data changes
                    monitoredItem1.DataChangeReceived += HandleDataChange;

                    // Set sampling interval (optional, in milliseconds)
                    monitoredItem1.SamplingInterval = 1000;

                    subscription.AddMonitoredItem(monitoredItem1);

                    // Apply the changes to activate the subscription
                    subscription.ApplyChanges();

                    Console.WriteLine($"Subscribed to NodeId: {nodeIdToSubscribe}. Press any key to exit.");
                    Console.ReadKey();

                    // Unsubscribe (optional, automatically done on client dispose)
                    subscription.RemoveMonitoredItem(monitoredItem);
                    subscription.RemoveMonitoredItem(monitoredItem1);
                    subscription.ApplyChanges();

                    Console.WriteLine("Unsubscribed and disconnected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }


        // Event handler for data changes
        private static void HandleDataChange(object sender, OpcDataChangeReceivedEventArgs e)
        {
            OpcMonitoredItem item = (OpcMonitoredItem)sender;
            Console.WriteLine($"Data Change from NodeId '{item.NodeId.Value}': {e.Item.Value} {e.Item.Notification.PublishTime}");
        }
    }
}
