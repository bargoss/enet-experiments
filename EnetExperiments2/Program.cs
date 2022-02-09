using System.Text;
using ENet;
//udp.port==6005
string ip = "127.0.0.1";
ushort port = 6005;
Library.Initialize();

#region server 
    var server = new Host();
    Address serversAddress = new Address();
    serversAddress.Port = port;
    server.Create(serversAddress, 10);
    Thread serverThread = new Thread(() =>
    {
        Event netEvent;
        while (true)
        {
            server.Service(15,out netEvent);
            if (netEvent.Type == EventType.None) { continue; }
            Console.WriteLine("Server recieved event: " + netEvent.Type);
            Thread.Sleep(100);
        }
    });
    serverThread.Start();
#endregion

Thread.Sleep(2000);

#region client
    var client = new Host();
    Address clientsAddress = new Address();
    clientsAddress.Port = port;
    clientsAddress.SetHost(ip);
    client.Create();
    Peer clientsPeer = client.Connect(clientsAddress);
    Thread clientThread = new Thread(() =>
    {
        Event netEvent;
        while (true)
        {
            client.Service(15,out netEvent);
            SendSomeText("hello");
            if (netEvent.Type == EventType.None) { continue; }
            Console.WriteLine("Client recieved event?: " + netEvent.Type);
            Thread.Sleep(1000);
            
        }
    });
    clientThread.Start();

    void SendSomeText(string text)
    {
        var packet = default(Packet);
        byte[] textBytes = Encoding.ASCII.GetBytes(text);
        packet.Create(textBytes);
        clientsPeer.Send(0, ref packet);
        int i = 0;
        string a = "3232";
    }
#endregion





while (true)
{
    Thread.Sleep(100);
}
