using System;
using System.Net.WebSockets;
using System.Threading;

namespace Battleship_v2.Networking;

public class WebSocketClient : NetworkPeer
{
	private ClientWebSocket webSocket { get; set; } = new ClientWebSocket();


	private async void receiveMessage()
	{
		byte[] aByteArray = new byte[1024];
		ArraySegment<byte> aBuffer = new ArraySegment<byte>(aByteArray);
		while (true)
		{
			WebSocketReceiveResult webSocketReceiveResult;
			try
			{
				webSocketReceiveResult = await webSocket.ReceiveAsync(aBuffer, m_CancelToken.Token);
			}
			catch (OperationCanceledException)
			{
				break;
			}
			if (webSocketReceiveResult != null)
			{
				string theMessage = NetworkPeer.DecodeString(aByteArray);
				addMessageToQueue(theMessage);
			}
			Array.Clear(aByteArray, 0, aByteArray.Length);
		}
	}

	private async void sendMessageAsync(string theMessage)
	{
		byte[] array = NetworkPeer.EncodeString(theMessage);
		await webSocket.SendAsync(new ArraySegment<byte>(array), WebSocketMessageType.Text, endOfMessage: false, default(CancellationToken));
	}

	public override void SendMessage(string theMessage)
	{
		sendMessageAsync(theMessage);
	}

	private async void connect(string theHostname)
	{
		Uri uri = new Uri($"ws://{theHostname}:{80}");
		await webSocket.ConnectAsync(uri, m_CancelToken.Token);
		base.m_NetworkThread = new Thread(receiveMessage);
		base.m_NetworkThread.Start();
	}

	public override void Connect(string theHostname)
	{
		connect(theHostname);
	}
}
