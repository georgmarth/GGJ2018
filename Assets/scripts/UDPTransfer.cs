using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPTransfer : MonoBehaviour {

    public static readonly Object lockObject = new Object();

    static UdpClient udpClient;
    string returnData = "";
    Thread receiveThread;

    public string remoteIP = "10.3.46.249";

    bool dataReceived = false;

    bool quit = false;

    //Coroutine receiveThread;
    Coroutine sendThread;

    [HideInInspector]
    private int Signal { get; set; }

	// Use this for initialization
	void Start () {

        Signal = 100;
        udpClient = new UdpClient(55557);
        //receiveThread = StartCoroutine(ReceiveThread());
        receiveThread = new Thread(new ThreadStart(ReceiveThread));
        receiveThread.Start();

        sendThread = StartCoroutine(SendThread());
	}

    void Ready()
    {

    }
	
	// Update is called once per frame
	void Update () {
        if (PlayManager.Instance.gameState == PlayManager.GameState.Running || PlayManager.Instance.gameState == PlayManager.GameState.Ready)
        {
            Signal = Mathf.Clamp((int)PlayManager.Instance.signalStrength, 0, 100);
            lock (lockObject)
            {
                if (dataReceived)
                {
                    UpdateSymbol(returnData);
                    dataReceived = false;
                }

            }
        }
	}

    void UpdateSymbol(string symbol)
    {
        //Debug.Log("symbol: " + symbol);
        Parse.MessageType messageType = Parse.GetType(symbol);
        //Debug.Log("Parsed symbol: " + Parse.GetMessage(symbol));
        switch (messageType)
        {
            case Parse.MessageType.Symbol:
                PlayManager.Instance.UpdateSymbol( Parse.GetMessage(symbol));
                break;
            case Parse.MessageType.Message:
                Debug.Log("symbol: " + symbol);
                PlayManager.Instance.UpdateMessage(Parse.GetMessage(symbol));
                break;
            case Parse.MessageType.GameState:
                PlayManager.Instance.UpdateGameState(Parse.GetMessage(symbol));
                break;
            default:
                break;
        }
        
    }

    private void ReceiveThread()
    {
      
        while (!quit)
        {
            try
            {
                IPEndPoint ReceiveEndPoint = new IPEndPoint(IPAddress.Any, 0);
                udpClient.Client.Blocking = true;
                byte[] receiveBytes = udpClient.Receive(ref ReceiveEndPoint);
                //Debug.Log(receiveBytes.Length);
                if (receiveBytes.Length > 0)
                {
                    lock (lockObject)
                    {
                        returnData = Encoding.ASCII.GetString(receiveBytes).TrimEnd('\0');
                        Debug.Log(returnData);
                        dataReceived = true;
                    }
                    //UpdateSymbol(returnData);
                }
            }
            catch (SocketException e)
            {
                Debug.LogError(e.Message);
            }
           

        }
    }

    private IEnumerator SendThread()
    {
        int sendSignal = 100;

        while (!quit)
        {
	    Debug.Log("sending an: " + remoteIP);
            IPEndPoint SendEndPoint = new IPEndPoint(IPAddress.Parse(remoteIP), 22222);

            if (PlayManager.Instance.gameState == PlayManager.GameState.Ready || PlayManager.Instance.gameState == PlayManager.GameState.Running)
            {
                
                byte[] sendmessage2 = Encoding.ASCII.GetBytes("g:r");
                udpClient.Send(sendmessage2, sendmessage2.Length, SendEndPoint);
                

                sendSignal = Signal;
                // send the signal strength
                byte[] sendmessage = Encoding.ASCII.GetBytes(sendSignal.ToString());
                //Debug.Log( 
                udpClient.Send(sendmessage, sendmessage.Length, SendEndPoint);
            
            //    );

                Debug.Log(remoteIP);
            }
            yield return 1f/30f;
        }

    }

    private void OnDestroy()
    {
        quit = true;
        if (sendThread != null)
        {
            StopCoroutine(sendThread);
        }
        if (receiveThread != null)
        {
            receiveThread.Abort();
            receiveThread.Join(1000);
        }
        if (udpClient != null)
            udpClient.Close();
    }
}
