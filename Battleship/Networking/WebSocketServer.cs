using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Networking;

public class WebSocketServer : NetworkPeer
{
    private TcpClient m_TcpClient;

    private NetworkStream m_Stream;

    private CancellationToken m_Token;

    private TcpListener m_Listener { get; set; }

    public WebSocketServer( CancellationToken theCancellationToken = default )
    {
        m_Token = theCancellationToken;
    }

    public async void Start()
    {
        m_Listener = new TcpListener( IPAddress.Any, 80 );
        m_Listener.AllowNatTraversal( allowed: true );
        m_Listener.Start();
        m_TcpClient = await m_Listener.AcceptTcpClientAsync();
        NetworkStream stream = m_TcpClient.GetStream();
        while ( !m_Token.IsCancellationRequested )
        {
            while ( !stream.DataAvailable )
            {
                Thread.Sleep( 25 );
                if ( m_Token.IsCancellationRequested )
                {
                    return;
                }
            }
            while ( m_TcpClient.Available < 3 )
            {
                if ( m_Token.IsCancellationRequested )
                {
                    return;
                }
            }
            byte[] array = new byte[m_TcpClient.Available];
            stream.Read( array, 0, array.Length );
            string @string = Encoding.UTF8.GetString( array );
            if ( Regex.IsMatch( @string, "^GET", RegexOptions.IgnoreCase ) )
            {
                performWebSocketHandshake( @string );
                continue;
            }
            _ = array[0];
            bool flag = ( array[1] & 0x80 ) != 0;
            _ = array[0];
            int num = 2;
            ulong num2 = array[1] & 0x7FuL;
            switch ( num2 )
            {
                case 126uL:
                    num2 = BitConverter.ToUInt16( new byte[2]
                    {
                    array[3],
                    array[2]
                    }, 0 );
                    num = 4;
                    break;
                case 127uL:
                    num2 = BitConverter.ToUInt64( new byte[8]
                    {
                    array[9],
                    array[8],
                    array[7],
                    array[6],
                    array[5],
                    array[4],
                    array[3],
                    array[2]
                    }, 0 );
                    num = 10;
                    break;
            }
            if ( num2 != 0L && flag )
            {
                byte[] array2 = new byte[num2];
                byte[] array3 = new byte[4]
                {
                    array[num],
                    array[num + 1],
                    array[num + 2],
                    array[num + 3]
                };
                num += 4;
                for ( ulong num3 = 0uL; num3 < num2; num3++ )
                {
                    array2[num3] = (byte)( array[num + (int)num3] ^ array3[num3 % 4] );
                }
                @string = DecodeString( array2 );
                addMessageToQueue( @string );
            }
        }
    }

    private void performWebSocketHandshake( string aMessage )
    {
        string text = Regex.Match( aMessage, "Sec-WebSocket-Key: (.*)" ).Groups[1].Value.Trim();
        text += "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        string text2 = Convert.ToBase64String( SHA1.Create().ComputeHash( Encoding.UTF8.GetBytes( text ) ) );
        byte[] bytes = Encoding.UTF8.GetBytes( "HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\nUpgrade: websocket\r\nSec-WebSocket-Accept: " + text2 + "\r\n\r\n" );
        m_TcpClient.GetStream().Write( bytes, 0, bytes.Length );
        notifyClientConnected();
    }

    public override void SendMessage( string theMessage )
    {
        if ( m_TcpClient.GetStream() != null && !string.IsNullOrEmpty( theMessage ) )
        {
            byte[] array = EncodeString( theMessage );
            byte[] array2 = array.Length < 126 ? new byte[2]
            {
                129,
                (byte)array.Length
            } : array.Length > 65536 ? new byte[10]
            {
                129,
                127,
                (byte)(array.Length >> 24),
                (byte)(array.Length >> 16),
                (byte)(array.Length >> 8),
                (byte)array.Length,
                (byte)(array.Length >> 24),
                (byte)(array.Length >> 16),
                (byte)(array.Length >> 8),
                (byte)array.Length
            } : new byte[4]
            {
                129,
                126,
                (byte)(array.Length >> 8),
                (byte)array.Length
            };
            byte[] array3 = new byte[array.Length + array2.Length];
            array2.CopyTo( array3, 0 );
            array.CopyTo( array3, array2.Length );
            m_TcpClient.GetStream().Write( array3, 0, array3.Length );
        }
    }

    public override void Connect( string theHostname = "" )
    {
        m_NetworkThread = new Thread( Start );
        m_NetworkThread.Start();
    }

    private void notifyClientConnected()
    {
        PeerConnected = true;
        Application.Current.Dispatcher.BeginInvoke( (Action)delegate
        {
            CommandManager.InvalidateRequerySuggested();
        } );
    }

    public override void Stop()
    {
        m_Listener.Stop();
        base.Stop();
    }
}
