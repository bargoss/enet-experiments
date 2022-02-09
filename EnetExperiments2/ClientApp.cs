using System.Text;

namespace EnetExperiments2;
using ENet;
public class ClientApp
{
    string ip = "127.0.0.1";
    ushort port = 6005;
    
    private Host client;
    private Peer peer;

    private string name;
    public void Init(string name)
    {
        this.name = name;
        client = new Host();
        Address clientsAddress = new Address();
        clientsAddress.Port = port;
        clientsAddress.SetHost(ip);
        client.Create();
        peer = client.Connect(clientsAddress);
        Thread clientThread = new Thread(() =>
        {
            Event netEvent;
            while (true)
            {
                client.Service(15,out netEvent);
                if (netEvent.Type != EventType.Receive) { continue; }

                var readBuffer = new byte[1024];
                netEvent.Packet.CopyTo(readBuffer);
                var readString = Encoding.UTF8.GetString(readBuffer);
                Console.WriteLine($"Client {name} recieved event: {netEvent.Type} , String value: {readString}");
                Thread.Sleep(100);
            }
        });
        clientThread.Start();
    }

    public void SendMsg(string text)
    {
        var packet = default(Packet);
        byte[] textBytes = Encoding.ASCII.GetBytes(text);
        packet.Create(textBytes, PacketFlags.Reliable); // its unreliable by default (PacketFlags.None)
        peer.Send(0, ref packet);
    }
}