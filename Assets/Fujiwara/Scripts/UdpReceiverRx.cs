using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UniRx;


public class UdpState: System.IEquatable<UdpState>
{
    //UDP通信の情報を収める。送受信ともに使える
    public IPEndPoint EndPoint { get; set; }
    public string UdpMsg { get; set; }

    public UdpState(IPEndPoint ep, string udpMsg)
    {
        this.EndPoint = ep;
        this.UdpMsg = udpMsg;
    }
    public override int GetHashCode()
    {
        return EndPoint.Address.GetHashCode();
    }

    public bool Equals(UdpState s)
    {
        if (s == null)
        {
            return false;
        }
        return EndPoint.Address.Equals(s.EndPoint.Address);
    }
}

public class UdpReceiverRx: MonoBehaviour
{
    [SerializeField] public int port = 8888;
    public IObservable<UdpState> udpSequence;
    public bool isReceive;
    private static UdpClient client;


    /*--------------------------------------------------------------------------
        @LifeCycleMethods
    --------------------------------------------------------------------------*/
    void OnEnable()
    {
        Connect();
    }


    void OnDisable()
    {
        Disconnect();
    }


    /*--------------------------------------------------------------------------
        @Methods
    --------------------------------------------------------------------------*/
    public void Connect()
    {
        udpSequence = Observable.Create<UdpState>(observer =>
        {
            Debug.Log(string.Format("udpSequence thread: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

            try
            {
                client = new UdpClient(port);
            }
            catch (SocketException ex)
            {
                observer.OnError(ex);
            }

            IPEndPoint remoteEP = null;
            client.EnableBroadcast = true;
            client.Client.ReceiveTimeout = 5000;

            while (!isReceive)
            {
                try
                {
                    remoteEP = null;
                    string receivedMsg = System.Text.Encoding.ASCII.GetString(client.Receive(ref remoteEP));
                    observer.OnNext(new UdpState(remoteEP, receivedMsg));
                }
                catch (SocketException)
                {
                    Debug.Log("UDPReceive Timeout");
                }
            }

            observer.OnCompleted();
            return null;
        })
        .SubscribeOn(Scheduler.ThreadPool)
        .Share();
        // .Publish()
        // .RefCount();
    }


    public void Disconnect()
    {
        isReceive = true;
        client.Client.Blocking = false;
        client.Close();
        client.Dispose();
        client = null;
    }
}
