using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour
{
    private string serverAddress = "127.0.0.1";
    private int serverSocketPort = 54321;
    private int clientSockedPort = 12345;
    
    int connectionId;
    int channelId;
    int hostId;

    void Start()
    {
        // Init Transport using default values.
        NetworkTransport.Init();

        // Create a connection config and add a Channel.
        ConnectionConfig config = new ConnectionConfig();
        channelId = config.AddChannel(QosType.ReliableSequenced);

        // Create a topology based on the connection config.
        HostTopology topology = new HostTopology(config, 10);

        // Create a host based on the topology we just created, and bind the socket to port.
        hostId = NetworkTransport.AddHost(topology, clientSockedPort, "127.0.0.1");
        Debug.Log("Socket Open. SocketId is: " + hostId);
        
        // Connect to the host with IP and port.
        byte error;
        connectionId = NetworkTransport.Connect(hostId, serverAddress, serverSocketPort, 0, out error);
        
        NetworkError networkError = (NetworkError) error;
        if (networkError != NetworkError.Ok) {
            Debug.LogError(string.Format("Unable to connect to {0}:{1}, Error: {2}", serverAddress, serverSocketPort, networkError));
        } else {
            Debug.Log(string.Format("Connected to {0}:{1} with hostId: {2}, connectionId: {3}, channelId: {4},", serverAddress, serverSocketPort, hostId, connectionId, channelId));
        }
    }

    private void Update()
    {
        int outHostId;
        int outConnectionId;
        int outChannelId;

        int receivedSize;
        byte error;
        byte[] buffer = new byte[256];
        NetworkEventType evt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);

        switch (evt)
        {
            case NetworkEventType.ConnectEvent:
            {
                OnConnect(outHostId, outConnectionId, (NetworkError)error);
                break;
            }
            case NetworkEventType.DisconnectEvent:
            {
                OnDisconnect(outHostId, outConnectionId, (NetworkError)error);
                break;
            }
            case NetworkEventType.DataEvent:
            {
                OnData(outHostId, outConnectionId, outChannelId, buffer, receivedSize, (NetworkError)error);
                break;
            }
            case NetworkEventType.BroadcastEvent:
            {
                OnBroadcast(outHostId, buffer, receivedSize, (NetworkError)error);
                break;
            }
            case NetworkEventType.Nothing:
                break;

            default:
                Debug.LogError("Unknown network message type received: " + evt);
                break;
        }
    }

    void OnConnect(int hostId, int connectionId, NetworkError error)
    {
        Debug.Log("OnConnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }

    void OnDisconnect(int hostId, int connectionId, NetworkError error)
    {
        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }

    void OnBroadcast(int hostId, byte[] data, int size, NetworkError error)
    {
        Debug.Log("OnBroadcast(hostId = " + hostId + ", data = "
            + data + ", size = " + size + ", error = " + error.ToString() + ")");
    }

    void OnData(int hostId, int connectionId, int channelId, byte[] data, int size, NetworkError error)
    {
        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", channelId = " + channelId + ", data = "
            + data + ", size = " + size + ", error = " + error.ToString() + ")");
    }
    
}