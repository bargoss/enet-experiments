namespace EnetExperiments2;
using ENet;

public class ClientApp
{
    private readonly string ip = "";
    private readonly ushort port = 1234;
    public void Run()
    {
        using (Host client = new Host()) {
            Address address = new Address();

            address.SetHost(ip);
            address.Port = port;
            client.Create();

            Peer peer = client.Connect(address);

            Event netEvent;

            while (!Console.KeyAvailable) {
                bool polled = false;

                while (!polled) {
                    if (client.CheckEvents(out netEvent) <= 0) {
                        if (client.Service(15, out netEvent) <= 0)
                            break;

                        polled = true;
                    }

                    switch (netEvent.Type) {
                        case EventType.None:
                            break;

                        case EventType.Connect:
                            Console.WriteLine("Client connected to server");
                            break;

                        case EventType.Disconnect:
                            Console.WriteLine("Client disconnected from server");
                            break;

                        case EventType.Timeout:
                            Console.WriteLine("Client connection timeout");
                            break;

                        case EventType.Receive:
                            Console.WriteLine("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                            netEvent.Packet.Dispose();
                            break;
                    }
                }
            }

            client.Flush();
        }
    }
}