using System;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;

namespace BBQ;

public class UserBBQLocal
{
    public string ID { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; } = string.Empty;
    public string IpAdress { get; set; } = "127.0.0.1";
    private int _port  = 5000;
    public int Port
    {
        get => _port;
        set
        {
            if (1024 <= value && value <= 65535)
            {
                _port = value;
            }
            else
            {
                _port = 5000;
            }
        }
    }

    public UserBBQLocal()
    {
        IpAdress = AutoDetectLocalIp();
    }
    public static string AutoDetectLocalIp()
    {
        try
        {
            var host =Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if(ip.AddressFamily==AddressFamily.InterNetwork && ip.ToString() != "127.0.0.1")
                {
                    return ip.ToString();
                }
            }
        }
        catch
        {

        }
        return "127.0.0.1";
    }

}