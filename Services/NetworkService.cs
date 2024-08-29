using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using Battleship_v2.Networking;
using Battleship_v2.Ships;

namespace Battleship_v2.Services;

public class NetworkService
{
	public const int PORT = 80;

	private object m_Lock = new object();

	private TcpListener m_Listener;

	public NetworkPeer NetworkPeer { get; set; }

	public NetworkStream Stream { get; private set; }

	public TcpListener Listener
	{
		get
		{
			lock (m_Lock)
			{
				if (m_Listener == null)
				{
					m_Listener = new TcpListener(IPAddress.Loopback, 80);
					m_Listener.Start();
				}
				return m_Listener;
			}
		}
	}

	public static NetworkService Instance { get; set; } = new NetworkService();


	private NetworkService()
	{
	}

	public void OpenServer()
	{
		NetworkPeer = new WebSocketServer();
		((WebSocketServer)NetworkPeer).Connect("");
	}

	public async void JoinServer(string theHostname)
	{
		await Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			NetworkPeer = new WebSocketClient();
		});
		NetworkPeer.Connect(theHostname);
	}

	public void Close()
	{
		if (NetworkPeer != null)
		{
			NetworkPeer.Stop();
		}
	}

	public static string ConvertShipListToStringRep(ObservableCollection<Ship> theShipList)
	{
		string text = string.Empty;
		foreach (Ship theShip in theShipList)
		{
			text += NetworkPeer.ConvertUshortToStringRep(theShip.ToBitField());
		}
		return text;
	}

	public static List<ushort> ConvertStringRepToUshortList(string theStringRep)
	{
		if (string.IsNullOrEmpty(theStringRep))
		{
			throw new ArgumentNullException("theStringRep");
		}
		if (theStringRep.Length % 2 != 0)
		{
			throw new ArgumentException("The String Rep has to be divisivble by two.");
		}
		List<ushort> list = new List<ushort>(theStringRep.Length / 2);
		for (int i = 0; i < theStringRep.Length; i += 2)
		{
			ushort item = NetworkPeer.ConvertStringRepToUshort(theStringRep.Substring(i, 2));
			list.Add(item);
		}
		return list;
	}
}
