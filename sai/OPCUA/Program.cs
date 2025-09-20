// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Opc.Ua.Client;
using Opc.UaFx;
using Opc.UaFx.Client;
using OPCUA.Data;
using System.Xml;


Console.WriteLine("Hello, World!");


// Replace with your OPC UA server URL
string serverUrl = "opc.tcp://CTHPGK3:49320";

// Replace with the NodeId you want to subscribe to (e.g., "ns=2;s=Message")
string nodeIdToSubscribe = "ns=2;s=Channel1.Device1.Station1Call";

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

        //// Create an OpcMonitoredItem for the desired node
        //var monitoredItem = new OpcMonitoredItem(nodeIdToSubscribe, OpcAttribute.Value);

        //// Attach an event handler for data changes
        //monitoredItem.DataChangeReceived += HandleDataChange;

        //// Set sampling interval (optional, in milliseconds)
        //monitoredItem.SamplingInterval = 1000;

        //// Add the monitored item to the subscription
        //subscription.AddMonitoredItem(monitoredItem);

        //var monitoredItem1 = new OpcMonitoredItem("ns=2;s=Channel1.Device1.Station2Call", OpcAttribute.Value);
        //monitoredItem1.DataChangeReceived += HandleDataChange;
        //monitoredItem1.SamplingInterval = 1000;
        //subscription.AddMonitoredItem(monitoredItem1);

        // Apply the changes to activate the subscription
        subscription.ApplyChanges();

        Console.WriteLine($"Subscribed to NodeId: {nodeIdToSubscribe}. Press any key to exit.");
        Console.ReadKey();

        for (int i = 0; i < opcMonitoredItems.Count(); i++)
        {
            // Unsubscribe (optional, automatically done on client dispose)
            subscription.RemoveMonitoredItem(opcMonitoredItems[i]);
        }

        // Unsubscribe (optional, automatically done on client dispose)
        //    subscription.RemoveMonitoredItem(monitoredItem);
        //subscription.RemoveMonitoredItem(monitoredItem1);
        subscription.ApplyChanges();

        Console.WriteLine("Unsubscribed and disconnected.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}


// Event handler for data changes
void HandleDataChange(object sender, OpcDataChangeReceivedEventArgs e)
{
    OpcMonitoredItem item = (OpcMonitoredItem)sender;
    Console.WriteLine($"Data Change from NodeId '{item.NodeId.Value}': {e.Item.Value} {e.Item.Notification.PublishTime}");

    using (var context = new OpcUaDatabaseContext())
    {
        var tagName = item.NodeId.Value;

        var machines = context.Machines.Where(p => p.MachineStatusTag.Equals(tagName)).FirstOrDefault();
        if (machines != null)
        {
            machines.MachineStatus = Convert.ToInt16(e.Item.Value.Value);
            context.SaveChanges();
        }
    }

}