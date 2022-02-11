using System.Text;

namespace EnetExperiments2;
using ENet;

public class ServerApp
{
    //string ip = "127.0.0.1";
    ushort port = 6005;
    
    private Host server;
    List<Peer> clients = new List<Peer>();
    public void Init()
    {
        server = new Host();
        Address serversAddress = new Address();
        serversAddress.Port = port;
        server.Create(serversAddress, 10);
        Thread serverThread = new Thread(() =>
        {
            Event netEvent;
            while (true)
            {
                server.Service(15,out netEvent);

                switch (netEvent.Type)
                {
                    case EventType.Receive:
                        var readBuffer = new byte[1024];
                        netEvent.Packet.CopyTo(readBuffer);
                        var readString = Encoding.UTF8.GetString(readBuffer);
                        Console.WriteLine("Server recieved event: " + netEvent.Type + ", String value: " + readString);
                        break;
                    case EventType.Connect:
                        Console.WriteLine("New client connected");
                        clients.Add(netEvent.Peer);
                        break;
                    case EventType.Disconnect:
                        Console.WriteLine("Client disconnected");
                        clients.Remove(netEvent.Peer);
                        break;
                }
                netEvent.Packet.Dispose();
                
                Thread.Sleep(100);
            }
        });
        serverThread.Start();
    }
    
    public void BroadcastMsg(string text)
    {
        var packet = default(Packet);
        byte[] textBytes = Encoding.ASCII.GetBytes(text);
        packet.Create(textBytes, PacketFlags.Reliable);
        server.Broadcast(0, ref packet);
    }
    /*
    I don't need to send different msgs to different clients for now
    public void SendMsg(string text, int peerId)
    {
        var packet = default(Packet);
        byte[] textBytes = Encoding.ASCII.GetBytes(text);
        packet.Create(textBytes);
        // send to peer here
    }
    */
}