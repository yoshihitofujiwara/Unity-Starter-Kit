using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class UdpSender : MonoBehaviour
{
    [SerializeField] public string host = "192.168.0.255";
    [SerializeField] public int port = 8888;
    private UdpClient client;



    /*--------------------------------------------------------------------------
        @Methods
    --------------------------------------------------------------------------*/
    public void Connect()
    {
        if(client != null)
        {
            return;
        }

        try
        {
            client = new UdpClient();
            client.Connect(host, port);
        }
        catch (Exception e)
        {
            Debug.Log("[UdpSender.Connect] " + e.ToString());
        }
    }


    public void DisConnect()
    {
        client.Close();
        client.Dispose();
        client = null;
    }


    public void Message(string message)
    {
        if(client == null || !client.Client.Connected)
        {
            return;
        }
        byte[] dgram = Encoding.UTF8.GetBytes(message);
        client.Send(dgram, dgram.Length);
    }


    public bool isConnected()
    {
        return client.Client.Connected;
    }
}
