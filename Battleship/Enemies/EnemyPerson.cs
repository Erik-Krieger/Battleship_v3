using System;
using System.Windows;
using Battleship.Services;
using Battleship.Utility;
using Networking;

namespace Battleship.Enemies;

public class EnemyPerson : Enemy
{
    public bool EventEnabled;

    public void InjectNetworkPeer( NetworkPeer theNetworkPeer )
    {
        theNetworkPeer.OnMessage += MakeMoveOnMessage;
    }

    public override Position NextMove()
    {
        NetworkPeer networkPeer = NetworkService.Instance.NetworkPeer;
        lock ( networkPeer.MessageQueue )
        {
            if ( networkPeer.GetMessage( out var theMessage ) )
            {
                return parsePositionString( theMessage );
            }
            EventEnabled = true;
            return null;
        }
    }

    public void MakeMoveOnMessage()
    {
        if ( EventEnabled && NetworkService.Instance.NetworkPeer.GetMessage( out var theMessage ) )
        {
            Position aPosition = parsePositionString( theMessage );
            EventEnabled = false;
            Application.Current.Dispatcher.BeginInvoke( (Action)delegate
            {
                GameManagerService.Instance.PlayNextMove( aPosition );
            } );
        }
    }

    private static Position parsePositionString( string thePositionString )
    {
        if ( string.IsNullOrEmpty( thePositionString ) )
        {
            return new Position();
        }
        string[] array = thePositionString.Split( ',' );
        if ( !int.TryParse( array[0], out var result ) )
        {
            return new Position();
        }
        if ( !int.TryParse( array[1], out var result2 ) )
        {
            return new Position();
        }
        return new Position( result, result2 );
    }
}
