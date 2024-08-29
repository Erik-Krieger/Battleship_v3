using System.Windows.Controls;
using System.Windows.Markup;
using Battleship.ViewModels;

namespace Battleship.Views;

public partial class AudioControlView : UserControl, IComponentConnector
{
    public AudioControlView()
    {
        InitializeComponent();
        DataContext = new AudioControlViewModel();
    }
}
