using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Battleship.Properties;

namespace Battleship.Utility;

public static class TileService
{
    private static List<Tile> m_TileCache = new List<Tile>( 20 );

    private static byte[] m_WaterTile;

    public static byte[] GetTile( TileType theType, TileOrientation theOrientation = TileOrientation.Up )
    {
        foreach ( Tile item in m_TileCache )
        {
            if ( item.Type == theType && item.Orientation == theOrientation )
            {
                return item.Data;
            }
        }

        if ( m_WaterTile == null )
        {
            m_WaterTile = Resources.Water_Sprite;
        }

        Image image;

        switch ( theType )
        {
            case TileType.Water:
                image = ImageFromByteArray( Resources.Water_Sprite );
                break;
            case TileType.Hit:
                image = mergeImages( m_WaterTile, Resources.Hit_Sprite );
                break;
            case TileType.Miss:
                image = mergeImages( m_WaterTile, Resources.Miss_Sprite );
                break;
            case TileType.PatrolBoat_1:
                image = mergeImages( m_WaterTile, Resources.PatrolBoat_1, theOrientation );
                break;
            case TileType.PatrolBoat_2:
                image = mergeImages( m_WaterTile, Resources.PatrolBoat_2, theOrientation );
                break;
            case TileType.Submarine_1:
                image = mergeImages( m_WaterTile, Resources.Submarine_1, theOrientation );
                break;
            case TileType.Submarine_2:
                image = mergeImages( m_WaterTile, Resources.Submarine_2, theOrientation );
                break;
            case TileType.Submarine_3:
                image = mergeImages( m_WaterTile, Resources.Submarine_3, theOrientation );
                break;
            case TileType.Carrier_1:
                image = mergeImages( m_WaterTile, Resources.Carrier_1, theOrientation );
                break;
            case TileType.Carrier_2:
                image = mergeImages( m_WaterTile, Resources.Carrier_2, theOrientation );
                break;
            case TileType.Carrier_3:
                image = mergeImages( m_WaterTile, Resources.Carrier_3, theOrientation );
                break;
            case TileType.Carrier_4:
                image = mergeImages( m_WaterTile, Resources.Carrier_4, theOrientation );
                break;
            case TileType.Carrier_5:
                image = mergeImages( m_WaterTile, Resources.Carrier_5, theOrientation );
                break;
            case TileType.Battleship_1:
                image = mergeImages( m_WaterTile, Resources.Battleship_1, theOrientation );
                break;
            case TileType.Battleship_2:
                image = mergeImages( m_WaterTile, Resources.Battleship_2, theOrientation );
                break;
            case TileType.Battleship_3:
                image = mergeImages( m_WaterTile, Resources.Battleship_3, theOrientation );
                break;
            case TileType.Battleship_4:
                image = mergeImages( m_WaterTile, Resources.Battleship_4, theOrientation );
                break;
            case TileType.Destroyer_1:
                image = mergeImages( m_WaterTile, Resources.Destroyer_1, theOrientation );
                break;
            case TileType.Destroyer_2:
                image = mergeImages( m_WaterTile, Resources.Destroyer_2, theOrientation );
                break;
            case TileType.Destroyer_3:
                image = mergeImages( m_WaterTile, Resources.Destroyer_3, theOrientation );
                break;
            default:
                image = null;
                break;
        }

        MemoryStream memoryStream = new MemoryStream();
        image.Save( memoryStream, ImageFormat.Png );
        byte[] array = memoryStream.ToArray();

        m_TileCache.Add( new Tile
        {
            Type = theType,
            Orientation = theOrientation,
            Data = array
        } );

        return array;
    }

    private static Image mergeImages( byte[] theBackgroundBytes, byte[] theForegroundBytes, TileOrientation theOrientation = TileOrientation.Up )
    {
        Image image = ImageFromByteArray( theBackgroundBytes );
        Image theForeground = ImageFromByteArray( theForegroundBytes );
        
        Graphics graphics = Graphics.FromImage( image );
        switch ( theOrientation )
        {
            case TileOrientation.Right:
                theForeground.RotateFlip( RotateFlipType.Rotate90FlipNone );
                break;
            case TileOrientation.Down:
                theForeground.RotateFlip( RotateFlipType.Rotate180FlipNone );
                break;
            case TileOrientation.Left:
                theForeground.RotateFlip( RotateFlipType.Rotate270FlipNone );
                break;
            default:
                throw new InvalidDataException( "The Rotation was invalid" );
            case TileOrientation.Up:
                break;
        }
        graphics.DrawImage( theForeground, 0, 0, 32, 32 );
        graphics.Dispose();
        return image;
    }

    public static Image ImageFromByteArray( byte[] imageBuffer )
    {
        Stream aStream = new MemoryStream( imageBuffer );

        return Image.FromStream( aStream );
    }
}
