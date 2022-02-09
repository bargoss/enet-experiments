namespace EnetExperiments2;
using ENet;

public static class ExtensionsPack
{
    // ToString extension for ENet.Packet
    public static byte[] buffer;
    
    public static string ToString2(this ENet.Packet packet)
    {
        if (buffer == null)
        {
            buffer = new byte[1024];
        }

        try
        {
            packet.CopyTo(buffer);
            return System.Text.Encoding.UTF8.GetString(buffer, 0, packet.Length);
        }
        catch (System.Exception)
        {
            return "couldn't read";
        }
    }
    
    
}