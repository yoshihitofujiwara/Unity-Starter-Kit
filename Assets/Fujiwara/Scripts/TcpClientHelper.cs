using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using UnityEngine;
using UniRx;



namespace Fujiwara {

    public class TcpClientHelper : MonoBehaviour
    {
        [SerializeField] public string serverIP = "192.168.11.111";
        [SerializeField] public int serverPort = 5039;

        public bool isPolling = false;
        public int pollingInterval = 1000;

        private TcpClient _client;





    private NetworkStream stream;
    private string receivedMessage = "";
    private int bytesReceived = 0;
    private int waitingMessagesFrequency = 8;
    private byte[] buffer = new byte[2048];



        // Events
        // 接続
        public IObservable<Unit> OnConnected { get { return _onConnected; } }
        Subject<Unit> _onConnected = new Subject<Unit>();

        // 切断
        public IObservable<Unit> OnDisconnected { get { return _onDisconnected; } }
        Subject<Unit> _onDisconnected = new Subject<Unit>();

        // 接続失敗
        public IObservable<Unit> OnConnectFail { get { return _onConnectFail; } }
        Subject<Unit> _onConnectFail = new Subject<Unit>();

        // メッセージ受信
        public IObservable<Unit> OnMessage { get { return _onMessage; } }
        Subject<Unit> _onMessage = new Subject<Unit>();


        /*--------------------------------------------------------------------------
            @methods
        --------------------------------------------------------------------------*/
        #region Unity Life Cycle Methods
        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            Disconnect();
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// 接続フラグ
        /// </summary>
        /// <value>bool</value>
        public bool isConnected
        {
            get
            {
                if (_client == null) return false;
                return _client.Connected;
            }
        }




        /// <summary>
        /// 接続
        /// </summary>
        public void Connect()
        {
            if(_client == null)
            {
                _client = new TcpClient(serverIP, serverPort);
            }
        }



        /// <summary>
        /// 切断
        /// </summary>
        public void Disconnect()
        {
            if (_client == null)
            {
                return;
            }
            if (_client.Connected)
            {
                _client.Close();
                _client.Dispose();
                // 切断メソッドたたく
            }
            _client = null;
        }




        /// <summary>
        /// 送信
        /// </summary>
        public void SendMessage()
        {
        }

        #endregion



        // private string[] DELIMITER = { "=====ANATOME_ML_MESSAGE=====" };



        // エラー
        // 接続状況
        // 受信


    }
}
