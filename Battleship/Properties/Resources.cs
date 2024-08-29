using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Properties;

[GeneratedCode( "System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0" )]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
    private static ResourceManager resourceMan;

    private static CultureInfo resourceCulture;

    [EditorBrowsable( EditorBrowsableState.Advanced )]
    internal static ResourceManager ResourceManager
    {
        get
        {
            if ( resourceMan == null )
            {
                resourceMan = new ResourceManager( "Battleship_v2.Properties.Resources", typeof( Resources ).Assembly );
            }
            return resourceMan;
        }
    }

    [EditorBrowsable( EditorBrowsableState.Advanced )]
    internal static CultureInfo Culture
    {
        get
        {
            return resourceCulture;
        }
        set
        {
            resourceCulture = value;
        }
    }

    internal static Bitmap Battleship_1 => (Bitmap)ResourceManager.GetObject( "Battleship_1", resourceCulture );

    internal static Bitmap Battleship_2 => (Bitmap)ResourceManager.GetObject( "Battleship_2", resourceCulture );

    internal static Bitmap Battleship_3 => (Bitmap)ResourceManager.GetObject( "Battleship_3", resourceCulture );

    internal static Bitmap Battleship_4 => (Bitmap)ResourceManager.GetObject( "Battleship_4", resourceCulture );

    internal static Bitmap Carrier_1 => (Bitmap)ResourceManager.GetObject( "Carrier_1", resourceCulture );

    internal static Bitmap Carrier_2 => (Bitmap)ResourceManager.GetObject( "Carrier_2", resourceCulture );

    internal static Bitmap Carrier_3 => (Bitmap)ResourceManager.GetObject( "Carrier_3", resourceCulture );

    internal static Bitmap Carrier_4 => (Bitmap)ResourceManager.GetObject( "Carrier_4", resourceCulture );

    internal static Bitmap Carrier_5 => (Bitmap)ResourceManager.GetObject( "Carrier_5", resourceCulture );

    internal static Bitmap Destroyer_1 => (Bitmap)ResourceManager.GetObject( "Destroyer_1", resourceCulture );

    internal static Bitmap Destroyer_2 => (Bitmap)ResourceManager.GetObject( "Destroyer_2", resourceCulture );

    internal static Bitmap Destroyer_3 => (Bitmap)ResourceManager.GetObject( "Destroyer_3", resourceCulture );

    internal static Bitmap hit_sprite => (Bitmap)ResourceManager.GetObject( "hit_sprite", resourceCulture );

    internal static Bitmap Miss => (Bitmap)ResourceManager.GetObject( "Miss", resourceCulture );

    internal static Bitmap PatrolBoat_1 => (Bitmap)ResourceManager.GetObject( "PatrolBoat_1", resourceCulture );

    internal static Bitmap PatrolBoat_2 => (Bitmap)ResourceManager.GetObject( "PatrolBoat_2", resourceCulture );

    internal static Bitmap Submarine_1 => (Bitmap)ResourceManager.GetObject( "Submarine_1", resourceCulture );

    internal static Bitmap Submarine_2 => (Bitmap)ResourceManager.GetObject( "Submarine_2", resourceCulture );

    internal static Bitmap Submarine_3 => (Bitmap)ResourceManager.GetObject( "Submarine_3", resourceCulture );

    internal static Bitmap water_sprite => (Bitmap)ResourceManager.GetObject( "water_sprite", resourceCulture );

    internal Resources()
    {
    }
}
