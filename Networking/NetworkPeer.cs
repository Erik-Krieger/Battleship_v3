using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Networking;

public abstract class NetworkPeer : PropertyChangeHandler
{
	public delegate void Notify();

	private protected CancellationTokenSource m_CancelToken = new CancellationTokenSource();

	private bool m_PeerConnected;

	private protected Thread m_NetworkThread { get; set; }

	public bool PeerConnected
	{
		get
		{
			return m_PeerConnected;
		}
		set
		{
			m_PeerConnected = value;
			NotifyPropertyChanged("PeerConnected");
		}
	}

	public List<string> MessageQueue { get; set; } = new List<string>();


	public event Notify OnMessage;

	public NetworkPeer()
	{
	}

	public bool GetMessage(out string theMessage)
	{
		theMessage = null;
		lock (MessageQueue)
		{
			if (MessageQueue == null || MessageQueue.Count == 0)
			{
				return false;
			}
			theMessage = MessageQueue.First();
			MessageQueue.RemoveAt(0);
			return true;
		}
	}

	private protected void addMessageToQueue(string theMessage)
	{
		if (string.IsNullOrEmpty(theMessage))
		{
			return;
		}
		if (theMessage.StartsWith("start-game") && NetworkService.Instance.NetworkPeer is WebSocketClient)
		{
			object jm = WindowManagerService.Instance.NavigationViewModel.SelectedViewModel;
			if (jm is JoinMenuViewModel)
			{
				string[] array = theMessage.Split(',');
				if (array.Length == 4 && int.TryParse(array[1], out var result))
				{
					GameManagerService.Instance.SetFirstTurnFromInt(result);
					string theStringRep = array[2];
					string theStringRep2 = array[3];
					List<ushort> enemyShipsUshortList = NetworkService.ConvertStringRepToUshortList(theStringRep);
					List<ushort> yourShipsUshortList = NetworkService.ConvertStringRepToUshortList(theStringRep2);
					Application.Current.Dispatcher.BeginInvoke((Action)delegate
					{
						((JoinMenuViewModel)jm).BeginGame(yourShipsUshortList, enemyShipsUshortList);
						CommandManager.InvalidateRequerySuggested();
					});
				}
				return;
			}
		}
		lock (MessageQueue)
		{
			MessageQueue.Add(theMessage);
		}
		this.OnMessage?.Invoke();
	}

	public abstract void SendMessage(string theMessage);

	public abstract void Connect(string theHostname);

	public virtual void Stop()
	{
		m_CancelToken.Cancel();
		try
		{
			m_NetworkThread.Abort();
		}
		catch (ThreadAbortException)
		{
		}
	}

	public static string ConvertUshortToStringRep(ushort theShort)
	{
		return $"{(char)(theShort >> 8)}{(char)(theShort & 0xFFu)}";
	}

	public static ushort ConvertStringRepToUshort(string theStringRep)
	{
		if (string.IsNullOrEmpty(theStringRep))
		{
			throw new ArgumentException("The string cannot be null or empty", "theStringRep");
		}
		if (theStringRep.Length != 2)
		{
			throw new ArgumentException($"The stirng has to have a length of 2. Your input: {theStringRep} with a length of {theStringRep.Length} is invalid", "theStringRep");
		}
		return (ushort)(((uint)theStringRep[0] << 8) + theStringRep[1]);
	}

	public static byte[] EncodeString(string theString)
	{
		string s = Convert.ToBase64String(Encoding.UTF8.GetBytes(theString));
		return Encoding.UTF8.GetBytes(s);
	}

	public static string DecodeString(byte[] theBytes)
	{
		string text = Encoding.UTF8.GetString(theBytes);
		int num = text.IndexOf('\0');
		if (num != -1)
		{
			text = text.Substring(0, num);
		}
		byte[] bytes = Convert.FromBase64String(text);
		return Encoding.UTF8.GetString(bytes);
	}
}
