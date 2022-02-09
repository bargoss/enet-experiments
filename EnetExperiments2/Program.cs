using System.Text;
using EnetExperiments2;

ENet.Library.Initialize();

ServerApp server = new ServerApp();
ClientApp client0 = new ClientApp();
ClientApp client1 = new ClientApp();
server.Init();
client0.Init("client 0");
client1.Init("client 1");

Thread.Sleep(1500);

client0.SendMsg("client 0 msg");

while (true)
{
    Thread.Sleep(100);
}
