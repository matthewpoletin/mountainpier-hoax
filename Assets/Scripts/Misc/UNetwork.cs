using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class UNetwork : MonoBehaviour
{
    private string serverAddress = "127.0.0.1";
    private int serverSocketPort = 54321;
    private int clientSockedPort = 12345;
    
    int connectionId;
    int channelId;
    int hostId;

    private void Start()
    {
        // Init Transport using default values.
        NetworkTransport.Init();

        // Create a connection config and add a Channel.
        ConnectionConfig config = new ConnectionConfig();
        channelId = config.AddChannel(QosType.ReliableSequenced);

        // Create a topology based on the connection config.
        HostTopology topology = new HostTopology(config, 10);

        // Create a host based on the topology we just created, and bind the socket to port.
        hostId = NetworkTransport.AddHost(topology, clientSockedPort);
        Debug.Log("Socket Open. SocketId is: " + hostId);
        
        // Connect to the host with IP and port.
        byte error;
        connectionId = NetworkTransport.Connect(hostId, serverAddress, serverSocketPort, 0, out error);
        
        NetworkError networkError = (NetworkError) error;
        if (networkError != NetworkError.Ok) {
            Debug.LogError($"Unable to connect to {serverAddress}:{serverSocketPort}, Error: {networkError}");
        } else {
            Debug.Log($"Connected to {serverAddress}:{serverSocketPort} with hostId: {hostId}, connectionId: {connectionId}, channelId: {channelId},");
        }
    }

    private void Update()
    {
        SendSocketMessage();
        
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

    public void SendSocketMessage() {
        // TODO: Check if connection is connected
        byte error;
        byte[] buffer = new byte[1024];
        Stream stream = new MemoryStream(buffer);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, "HelloServer");
        int bufferSize = 1024;
        NetworkTransport.Send(hostId, connectionId, channelId, buffer, bufferSize, out error);
    }
    
    private static void OnConnect(int hostId, int connectionId, NetworkError error)
    {
        Debug.Log("OnConnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }

    private static void OnDisconnect(int hostId, int connectionId, NetworkError error)
    {
        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }

    private static void OnBroadcast(int hostId, byte[] data, int size, NetworkError error)
    {
        Debug.Log("OnBroadcast(hostId = " + hostId + ", data = "
            + data + ", size = " + size + ", error = " + error.ToString() + ")");
    }

    private static void OnData(int hostId, int connectionId, int channelId, byte[] data, int size, NetworkError error)
    {
        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", channelId = " + channelId + ", data = "
            + data + ", size = " + size + ", error = " + error.ToString() + ")");
    }
    
}