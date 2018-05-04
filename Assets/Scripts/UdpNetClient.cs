using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpNetClient : MonoBehaviour
{
	private const int ClientPort = 12345;
	private const int ServerPort = 54321;
	private const string ServerAddress = "127.0.0.1";

	private UdpClient _udpClient;
	private IPEndPoint _sendEndPoint;
	private IPEndPoint _recieveEndPoint;

	private readonly Queue _pQueue = Queue.Synchronized(new Queue());
	
	private Thread _thread;

	private void Start()
	{
		Connect();
		Send("Unity game connecting");
		
		// Setup thread for receiving messages
		Application.runInBackground = true;
		_thread = new Thread(Recieve) {IsBackground = true};
		_thread.Start();
	}

	private void OnApplicationQuit()
	{
		Disconnect();
	}

	private void Update()
	{
		lock (_pQueue.SyncRoot)
		{
			if (_pQueue.Count > 0)
			{
				var p = _pQueue.Dequeue();
				Debug.Log($"Received message: {p}");
			}
		}
	}

	private void Connect()
	{
		//IPEndPoint object will allow us to read datagrams sent from any source.
		_recieveEndPoint = new IPEndPoint(IPAddress.Any, ClientPort);
		_udpClient = new UdpClient(_recieveEndPoint);
		
		_sendEndPoint = new IPEndPoint(IPAddress.Parse(ServerAddress), ServerPort);
		_udpClient.Connect(_sendEndPoint);
		_udpClient.Client.ReceiveTimeout = 1000;
		_udpClient.Client.Blocking = false;
	}

	private void Disconnect()
	{
		if (_udpClient != null)
		{
			if (_udpClient.Available == 0)
			{
				Debug.Log("Closing udp conneciton");
				_udpClient.Close();
			}
		}
	}

	private void Recieve()
	{
		var data = new byte[0];
		while (true)
		{
			try
			{
				data = _udpClient.Receive(ref _recieveEndPoint);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
//				Disconnect();
				return;
			}
			var json = Encoding.UTF8.GetString(data);
			_pQueue.Enqueue(json);
		}
	}

	public void Send(string msg)
	{
		var sendBytes = Encoding.UTF8.GetBytes(msg);
		_udpClient?.Send(sendBytes, sendBytes.Length);
	}
	
}
